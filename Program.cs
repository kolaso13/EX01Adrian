using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static System.Console;


public class InstitutoContext : DbContext
{
    public DbSet<Modulo> Modulos { get; set; }
    public DbSet<Matricula> Matriculas { get; set; }
    public DbSet<Alumno> Alumnos { get; set; }
    public string connString { get; private set; }

    public InstitutoContext()
    {
        var database = "EF01Adrian"; // "EF{XX}Nombre" => EF00Santi
        connString = $"Server=185.60.40.210\\SQLEXPRESS,58015;Database={database};User Id=sa;Password=Pa88word;MultipleActiveResultSets=true";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(connString);
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Matricula>().HasIndex(m => new
        {
            m.AlumnoId,
            m.ModuloId
        }).IsUnique();
    }
}
public class Alumno
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int AlumnoId { get; set; } 
    public string Nombre { get; set; }
    public int Edad { get; set; }
    public int Efectivo { get; set; }
    public string Pelo { get; set; }
    public List<Matricula> Matriculas { get; } = new List<Matricula>();
        
    public override string ToString() => $"{AlumnoId}: {Nombre}: {Edad}: {Efectivo}: {Pelo} ";
}
public class Modulo
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ModuloId { get; set; }
    public string Titulo { get; set; }
    public int Credito { get; set; }
    public int Curso { get; set; }
    public List<Matricula> Matriculas { get; } = new List<Matricula>();
        
    public override string ToString() => $"{ModuloId}: {Titulo}: {Credito}: {Curso} ";
}
public class Matricula
{
    public int MatriculaId { get; set; }
    public int AlumnoId { get; set; }
    public int ModuloId { get; set; }

    public Alumno Alumnos { get; set; }
    public Modulo Modulos { get; set; }

    public override string ToString() => $"{AlumnoId}: {ModuloId} ";
}

class Program
{
    static void GenerarDatos()
    {
        using (var db = new InstitutoContext())
        {
            // Borrar todo
            db.Matriculas.RemoveRange(db.Matriculas);
            db.Alumnos.RemoveRange(db.Alumnos);
            db.Modulos.RemoveRange(db.Modulos);
            db.SaveChanges();
            // Añadir Alumnos
            // Id de 1 a 7
            db.Add(new Alumno {AlumnoId=1, Nombre ="Adrian", Edad=20, Efectivo=5000000, Pelo="Rubio"});
            db.Add(new Alumno {AlumnoId=2, Nombre ="Peter", Edad=23, Efectivo=100, Pelo="castaño"});
            db.Add(new Alumno {AlumnoId=3, Nombre ="Manuel", Edad=22, Efectivo=50, Pelo="Rubio"});
            db.Add(new Alumno {AlumnoId=4, Nombre ="Carlos", Edad=21, Efectivo=4, Pelo="castaño"});
            db.Add(new Alumno {AlumnoId=5, Nombre ="Juan", Edad=25, Efectivo=50, Pelo="moreno"});
            db.Add(new Alumno {AlumnoId=6, Nombre ="Leslie", Edad=27, Efectivo=30, Pelo="moreno"});
            db.Add(new Alumno {AlumnoId=7, Nombre ="Maria", Edad=29, Efectivo=5, Pelo="castaño"});
            db.SaveChanges();
            // Añadir Módulos
            // Id de 1 a 10
          
            db.Add(new Modulo {ModuloId=1, Titulo ="Quimica", Credito=20000, Curso=5});
            db.Add(new Modulo {ModuloId=2, Titulo ="Fisica", Credito=200, Curso=6});
            db.Add(new Modulo {ModuloId=3, Titulo ="Mates", Credito=200, Curso=4});
            db.Add(new Modulo {ModuloId=4, Titulo ="Filo", Credito=178, Curso=1});
            db.Add(new Modulo {ModuloId=5, Titulo ="Gimnasia", Credito=403, Curso=3});
            db.Add(new Modulo {ModuloId=6, Titulo ="Plastica", Credito=121, Curso=1});
            db.Add(new Modulo {ModuloId=7, Titulo ="Infor", Credito=2313, Curso=2});
            db.Add(new Modulo {ModuloId=8, Titulo ="Euskera", Credito=13231, Curso=4});
            db.Add(new Modulo {ModuloId=9, Titulo ="Ingles", Credito=123123, Curso=3});
            db.Add(new Modulo {ModuloId=10, Titulo ="Lengua", Credito=2, Curso=1});
            db.SaveChanges();

            // Matricular Alumnos en Módulos
            var alumnos = db.Alumnos.OrderBy(b => b.AlumnoId);
                foreach(var i in alumnos)
                {
                    var modulos = db.Modulos.OrderBy(b=> b.ModuloId).Select(e => e.ModuloId);
                    foreach(var j in modulos)
                    {
                            db.Add(new Matricula{AlumnoId = i.AlumnoId, ModuloId = j});
                    }
                }
            db.SaveChanges();
        }
    }

    static void BorrarMatriculaciones()
    {
        using (var db = new InstitutoContext())
        {
            // Borrar las matriculas d
            // AlumnoId multiplo de 3 y ModuloId Multiplo de 2;
            var alumnos = db.Alumnos.OrderBy(b => b.AlumnoId);
            var alumnosMultiplos3 = alumnos.Where(o => o.AlumnoId % 3 == 0);
            var modulos = db.Modulos.OrderBy(m=> m.ModuloId);
            var modulosMultiplos2 = modulos.Where(h => h.ModuloId % 2 == 0);
            var matriculas = db.Matriculas.OrderBy(b => b.MatriculaId);
            foreach(var i in matriculas){
                db.Modulos.Remove(db.Matriculas).while (true)
                {
                     
                }
            }
            // AlumnoId multiplo de 2 y ModuloId Multiplo de 5;

        }   
    }
    static void RealizarQuery()
    {
        using (var db = new InstitutoContext())
        {
            // Las queries que se piden en el examen

        }
    }

    static void Main(string[] args)
    {
        GenerarDatos();
        BorrarMatriculaciones();
        RealizarQuery();
    }

}