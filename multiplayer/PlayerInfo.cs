using System;

public class PlayerInfo
{
    public string Name { get; set; }
    public int Id { get; set; }

    public bool IsSpawned{get; set;}

    public override string ToString(){
        return $" Id : {Id} - Name : {Name} - IsSpawned : {IsSpawned}";
    }

}