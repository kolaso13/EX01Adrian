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
            foreach (var i in db.Matriculas)
            {
                if (i.AlumnoId % 3 == 0 && i.ModuloId % 2 == 0)
                {
                    db.Matriculas.Remove(i);
                }
            }

            db.SaveChanges();
            // AlumnoId multiplo de 2 y ModuloId Multiplo de 5;
            foreach (var j in db.Matriculas)
            {
                if (j.AlumnoId % 2 == 0 && j.ModuloId % 5 == 0)
                {
                    db.Matriculas.Remove(j);
                }
            }
            db.SaveChanges();
        }   
    }
    static void RealizarQuery()
    {
        using (var db = new InstitutoContext())
        {
            // Las queries que se piden en el examen
            
            //1 Filtering (cada 1)
            var q1 = db.Matriculas.Where(o => o.MatriculaId < 84);
       
            //1 Anomnimous tupe (cada 1)
            var q2 = db.Alumnos.Select(o => new{AlumnoId = o.AlumnoId,Efectivo = o.Efectivo});
            
            //3 Ordenring  (cada 1)
            var q3 = db.Modulos.OrderBy(o => o.Curso);
            var q4 = db.Alumnos.OrderByDescending(o => o.Edad);
            var q5 = db.Alumnos.OrderBy(o => o.AlumnoId).ThenByDescending(o => o.Efectivo);
            
            //2 Joining
            var q6 = db.Alumnos.Join(db.Matriculas,c => c.AlumnoId,o => o.AlumnoId,(c, o) => new{c.AlumnoId,c.Nombre,o.MatriculaId,o.ModuloId});
            var q7 = db.Modulos.Join(db.Matriculas,c => c.ModuloId,o => o.ModuloId,(c, o) => new{c.ModuloId,c.Titulo,o.MatriculaId,o.AlumnoId});
            
            //3 Grouping
            var q8 = db.Matriculas.GroupBy(o => o.MatriculaId);
            var q9 = db.Modulos.GroupBy(o => o.ModuloId);
            var q10 = db.Alumnos.GroupBy(o => o.AlumnoId);

            //2 Paging  (cada 1)
            var q11 = db.Alumnos.Where(o => o.AlumnoId == 7).Take(3);
            var q12 = (from o in db.Modulos where o.ModuloId == 5 orderby o.Credito select o).Skip(2).Take(2);
            
            //5 Elements Opertators (cada 1)
            var q13 = db.Alumnos.Single(c => c.AlumnoId == 2);
            var q14 = db.Modulos.SingleOrDefault(c => c.ModuloId == 3);
            var q15 = db.Alumnos.Where(c => c.AlumnoId == 6).DefaultIfEmpty(new Alumno()).Single();
            var q16 = db.Modulos.Where(o => o.ModuloId == 1).OrderBy(o => o.Credito).Last();
            var q17 = db.Modulos.Where(c => c.ModuloId == 4).Select(o => o.ModuloId).SingleOrDefault();
            
            //Conversiones
            //1 ToArray
            string[] names = (from c in db.Alumnos select c.Nombre).ToArray();
            //1 ToDicctionary
            Dictionary<int, Alumno> col = db.Alumnos.ToDictionary(c => c.AlumnoId);
            Dictionary<string, double> customerOrdersWithMaxCost = (from oc in
            
            (from o in db.Matriculas
            join c in db.Alumnos on o.AlumnoId equals c.AlumnoId
            select new { c.Nombre, o.MatriculaId })
            
            group oc by oc.Nombre into g
            select g).ToDictionary(g => g.Key, g => g.Max(oc => oc.));

            //1 ToList
            List<Modulo> ordersOver10 = (from o in db.Modulos where o.Credito > 10 orderby o.Credito).ToList();
            //2 ILookup
            ILookup<int, string> customerLookup = db.Alumnos.ToLookup(c => c.AlumnoId, c => c.Nombre);
            
        }
    }

    static void Main(string[] args)
    {
        GenerarDatos();
        BorrarMatriculaciones();
        RealizarQuery();
    }

}