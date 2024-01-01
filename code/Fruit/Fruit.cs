namespace Soyka;

public struct Fruit : IEquatable<Fruit>
{
	/// <summary>
	/// List of all fruits.
	/// </summary>
	/// <returns></returns>
	public static List<Fruit> All = new List<Fruit>()
	{
		new() { Name = "Peajak", Size = 5f, Texture = "materials/peajak.png" },
		new() { Name = "Blueberryjak", Size = 7.5f, Texture = "materials/blueberryjak.png" },
		new() { Name = "Orangejak", Size = 20f, Texture = "materials/orangejak.png" },
		new() { Name = "Lettucejak", Size = 40f, Texture = "materials/lettucejak.png" },
		new() { Name = "Meatballjak", Size = 50f, Texture = "materials/meatjak.png" },
	};

	public string Name;
	public float Size;
	public string Texture;

	/// <summary>
	/// Gets the next fruit, is capped within the method.
	/// </summary>
	/// <returns></returns>
	public Fruit GetNext() 
		=> All[Math.Clamp( All.IndexOf( this ) + 1, 0, All.Count - 1 )];

	/// <summary>
	/// Is this fruit the last possible fruit?
	/// </summary>
	/// <returns></returns>
	public bool IsLast()
		=> All.IndexOf( this ) >= All.Count - 1;

	// IEquatable
	public static bool operator ==( Fruit left, Fruit right ) =>
        Equals( left, right );

    public static bool operator !=( Fruit left, Fruit right) =>
        !Equals( left, right );

   public override bool Equals( object obj ) 
   		=> (obj is Fruit fruit) 
	   		&& Equals( fruit );

    public bool Equals( Fruit other ) 
		=> Name == other.Name
		&& Texture == other.Texture;

    public override int GetHashCode()
		=> (Name, Size, Texture).GetHashCode();
}
