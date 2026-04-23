namespace BarberiaTurnos.BE
{
    public class RolBE
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public override string ToString() => Nombre;
    }
}
