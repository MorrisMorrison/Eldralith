using System;

public class PlayerInfo
{
    public string Name { get; set; }
    public int Id { get; set; }

    public override string ToString(){
        return $"Name : {Name}, Id : {Id}";
    }

}