namespace Soyka;

public struct Fruit : IEquatable<Fruit>
{
	/// <summary>
	/// List of all fruits.
	/// </summary>
	/// <typeparam name="Fruit"></typeparam>
	/// <returns></returns>
	public static List<Fruit> All = new List<Fruit>()
	{
		new() { Name = "Strawberry", Size = 5f, Texture = "" },
		new() { Name = "Blueberry", Size = 7.5f, Texture = "" },
		new() { Name = "Apple", Size = 15f, Texture = "" },
		new() { Name = "Orange", Size = 20f, Texture = "" },
		new() { Name = "Melon", Size = 40f, Texture = "" },
		new() { Name = "Watermelon", Size = 50f, Texture = "" },
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
