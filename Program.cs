using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class Donante
{
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Provincia { get; set; } = string.Empty;
    public string Canton { get; set; } = string.Empty;
    public string Distrito { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string GrupoSanguineo { get; set; } = string.Empty;
    public string FactorRH { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{Nombre},{Correo},{Telefono},{Provincia},{Canton},{Distrito},{Direccion},{GrupoSanguineo},{FactorRH}";
    }
}

class BancoDeSangre
{
    private const string Archivo = "banco_sangre.txt";
    private List<Donante> donantes;

    public BancoDeSangre()
    {
        donantes = new List<Donante>();
        CargarDatos();
    }

    private void CargarDatos()
    {
        if (File.Exists(Archivo))
        {
            var lineas = File.ReadAllLines(Archivo);
            donantes = lineas.Where(linea => !string.IsNullOrWhiteSpace(linea))
                             .Select(linea => linea.Split(','))
                             .Where(datos => datos.Length == 9)
                             .Select(datos => new Donante
                             {
                                 Nombre = datos[0],
                                 Correo = datos[1],
                                 Telefono = datos[2],
                                 Provincia = datos[3],
                                 Canton = datos[4],
                                 Distrito = datos[5],
                                 Direccion = datos[6],
                                 GrupoSanguineo = datos[7],
                                 FactorRH = datos[8]
                             }).ToList();
        }
        else
        {
            Console.WriteLine("El archivo de datos no existe. Se creará uno nuevo.");
        }
    }

    private void GuardarDatos()
    {
        File.WriteAllLines(Archivo, donantes.Select(d => d.ToString()));
    }

    public void AgregarDonante(Donante donante)
    {
        donantes.Add(donante);
        GuardarDatos();
    }

    public Donante BuscarDonante(string nombre)
    {
        return donantes.FirstOrDefault(d => d.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
    }

    public void EditarDonante(string nombre)
    {
        var donante = BuscarDonante(nombre);
        if (donante != null)
        {
            Console.Write("Nuevo Correo: "); donante.Correo = Console.ReadLine();
            Console.Write("Nuevo Teléfono: "); donante.Telefono = Console.ReadLine();
            Console.Write("Nueva Provincia: "); donante.Provincia = Console.ReadLine();
            Console.Write("Nuevo Cantón: "); donante.Canton = Console.ReadLine();
            Console.Write("Nuevo Distrito: "); donante.Distrito = Console.ReadLine();
            Console.Write("Nueva Dirección: "); donante.Direccion = Console.ReadLine();
            Console.Write("Nuevo Grupo Sanguíneo: "); donante.GrupoSanguineo = Console.ReadLine();
            Console.Write("Nuevo Factor RH (+/-): "); donante.FactorRH = Console.ReadLine();
            GuardarDatos();
        }
        else
        {
            Console.WriteLine("Donante no encontrado.");
        }
    }

    public void ContarTiposDeSangre()
    {
        var conteo = donantes.GroupBy(d => d.GrupoSanguineo + " " + d.FactorRH)
                              .ToDictionary(g => g.Key, g => g.Count());
        Console.WriteLine("Cantidad de cada tipo de sangre:");
        foreach (var tipo in conteo)
        {
            Console.WriteLine($"{tipo.Key}: {tipo.Value}");
        }
    }
}

class Program
{
    static void Main()
    {
        BancoDeSangre banco = new BancoDeSangre();

        while (true)
        {
            Console.WriteLine("\n1. Agregar Donante");
            Console.WriteLine("2. Buscar Donante");
            Console.WriteLine("3. Editar Donante");
            Console.WriteLine("4. Contar Tipos de Sangre");
            Console.WriteLine("5. Salir");
            Console.Write("Seleccione una opción: ");
            if (!int.TryParse(Console.ReadLine(), out int opcion))
                continue;

            if (opcion == 5) break;

            switch (opcion)
            {
                case 1:
                    Console.Write("Nombre: "); string nombre = Console.ReadLine();
                    Console.Write("Correo: "); string correo = Console.ReadLine();
                    Console.Write("Teléfono: "); string telefono = Console.ReadLine();
                    Console.Write("Provincia: "); string provincia = Console.ReadLine();
                    Console.Write("Cantón: "); string canton = Console.ReadLine();
                    Console.Write("Distrito: "); string distrito = Console.ReadLine();
                    Console.Write("Dirección: "); string direccion = Console.ReadLine();
                    Console.Write("Grupo Sanguíneo: "); string grupo = Console.ReadLine();
                    Console.Write("Factor RH (+/-): "); string rh = Console.ReadLine();
                    banco.AgregarDonante(new Donante { Nombre = nombre, Correo = correo, Telefono = telefono, Provincia = provincia, Canton = canton, Distrito = distrito, Direccion = direccion, GrupoSanguineo = grupo, FactorRH = rh });
                    Console.WriteLine("Donante agregado exitosamente.");
                    break;
                case 2:
                    Console.Write("Ingrese el nombre del donante: ");
                    nombre = Console.ReadLine();
                    Donante encontrado = banco.BuscarDonante(nombre);
                    Console.WriteLine(encontrado != null ? encontrado.ToString() : "No encontrado.");
                    break;
                case 3:
                    Console.Write("Ingrese el nombre del donante a editar: ");
                    nombre = Console.ReadLine();
                    banco.EditarDonante(nombre);
                    break;
                case 4:
                    banco.ContarTiposDeSangre();
                    break;
            }
        }
    }
}