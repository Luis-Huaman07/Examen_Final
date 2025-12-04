using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen_Final
{
    internal class Program
    {
        // Datos globales
        static string[] combos = new string[3];
        static double[] precios = new double[3];

        static string[,] reservas = new string[2, 20];
        static int[] contador = new int[2];
        static void Main(string[] args)
        {
            // Se pide menú solo una vez al iniciar
            DefinirMenuDeCombos();

            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("=== SISTEMA DE RESERVAS ===");
                Console.WriteLine("1. Mostrar menú de combos");
                Console.WriteLine("2. Registrar reserva");
                Console.WriteLine("3. Cancelar reserva");
                Console.WriteLine("4. Listar reservas por turno");
                Console.WriteLine("5. Calcular ingresos");
                Console.WriteLine("6. Buscar reserva por nombre");
                Console.WriteLine("7. Salir");
                Console.Write("Seleccione una opción: ");

                opcion = int.Parse(Console.ReadLine());

                switch (opcion)
                {
                    case 1: MostrarMenu(); break;
                    case 2: RegistrarReserva(); break;
                    case 3: CancelarReserva(); break;
                    case 4: ListarReservas(); break;
                    case 5: CalcularIngresos(); break;
                    case 6: BuscarReserva(); break;
                }

                if (opcion != 7)
                {
                    Console.WriteLine("\nPresione ENTER para continuar...");
                    Console.ReadKey();
                }

            } while (opcion != 7);
        }

        // ----------------------------
        // DEFINIR MENÚ DE COMBOS
        // ----------------------------
        static void DefinirMenuDeCombos()
        {
            Console.WriteLine("=== Definir menú de combos ===");
            for (int i = 0; i < combos.Length; i++)
            {
                Console.Write($"Nombre del combo {i + 1}: ");
                combos[i] = Console.ReadLine();

                Console.Write($"Precio del combo {i + 1}: ");
                precios[i] = double.Parse(Console.ReadLine());

                Console.WriteLine();
            }
        }

        static void MostrarMenu()
        {
            Console.WriteLine("=== Menú de combos ===");
            for (int i = 0; i < combos.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {combos[i]} - S/{precios[i]:0.00}");
            }
        }

        static void RegistrarReserva()
        {
            Console.WriteLine("=== Registrar reserva ===");

            Console.Write("Turno (0 = mañana, 1 = tarde): ");
            int turno = int.Parse(Console.ReadLine());

            if (contador[turno] >= 20)
            {
                Console.WriteLine("No hay cupos disponibles en este turno.");
                return;
            }

            MostrarMenu();
            Console.Write("Seleccione combo (1-3): ");
            int combo = int.Parse(Console.ReadLine()) - 1;

            Console.Write("Nombre del estudiante: ");
            string nombre = Console.ReadLine();

            reservas[turno, contador[turno]] = nombre + " - " + combos[combo];
            contador[turno]++;

            Console.WriteLine("Reserva registrada correctamente.");
        }

        static void CancelarReserva()
        {
            Console.WriteLine("=== Cancelar reserva ===");

            Console.Write("Turno (0 = mañana, 1 = tarde): ");
            int turno = int.Parse(Console.ReadLine());

            ListarReservas(turno);

            Console.Write("Ingrese número de reserva a cancelar: ");
            int index = int.Parse(Console.ReadLine()) - 1;

            if (index < 0 || index >= contador[turno])
            {
                Console.WriteLine("Índice inválido.");
                return;
            }

            for (int i = index; i < contador[turno] - 1; i++)
            {
                reservas[turno, i] = reservas[turno, i + 1];
            }

            reservas[turno, contador[turno] - 1] = null;
            contador[turno]--;

            Console.WriteLine("Reserva cancelada.");
        }

        static void ListarReservas()
        {
            Console.Write("Turno a listar (0=mañana, 1=tarde): ");
            int turno = int.Parse(Console.ReadLine());

            ListarReservas(turno);
        }

        static void ListarReservas(int turno)
        {
            Console.WriteLine($"=== Reservas del turno {(turno == 0 ? "mañana" : "tarde")} ===");

            if (contador[turno] == 0)
            {
                Console.WriteLine("No hay reservas.");
                return;
            }

            for (int i = 0; i < contador[turno]; i++)
            {
                Console.WriteLine($"{i + 1}. {reservas[turno, i]}");
            }
        }

        static void CalcularIngresos()
        {
            double totalGeneral = 0;

            Console.WriteLine("=== Ingresos por turno ===");

            for (int turno = 0; turno < 2; turno++)
            {
                double totalTurno = 0;

                for (int i = 0; i < contador[turno]; i++)
                {
                    for (int c = 0; c < combos.Length; c++)
                    {
                        if (reservas[turno, i].Contains(combos[c]))
                            totalTurno += precios[c];
                    }
                }

                totalGeneral += totalTurno;

                Console.WriteLine($"Turno {(turno == 0 ? "mañana" : "tarde")}: S/{totalTurno:0.00}");
            }

            Console.WriteLine($"Total general: S/{totalGeneral:0.00}");
        }

        static void BuscarReserva()
        {
            Console.Write("Ingrese nombre a buscar: ");
            string nombre = Console.ReadLine();

            bool encontrado = false;

            for (int turno = 0; turno < 2; turno++)
            {
                for (int i = 0; i < contador[turno]; i++)
                {
                    if (reservas[turno, i].ToLower().Contains(nombre.ToLower()))
                    {
                        Console.WriteLine($"Encontrado en turno {(turno == 0 ? "mañana" : "tarde")} → {reservas[turno, i]}");
                        encontrado = true;
                    }
                }
            }

            if (!encontrado)
                Console.WriteLine("No se encontró ninguna reserva con ese nombre.");
        }
    }
}
