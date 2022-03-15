internal readonly record struct Person(string Name, int Age);

internal abstract class Shape
{
    public abstract double Area { get; }

    public string Name { get; set; }
} 
internal sealed class Circle : Shape
{
    public double Diameter { get; set; }
    public override double Area => this.Radius * Math.PI;
    public double Radius => this.Diameter / 2;
}

internal sealed class Recatngle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }
    public override double Area => this.Width * this.Height;
}