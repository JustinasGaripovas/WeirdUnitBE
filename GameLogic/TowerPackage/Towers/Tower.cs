using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using WeirdUnitBE.GameLogic;
using System.Reflection;
using WeirdUnitBE.GameLogic.Services.Interfaces;

namespace WeirdUnitBE.GameLogic.TowerPackage.Towers
{
    [Serializable]
    public abstract class Tower : IPrototype
    {
        private string _owner = String.Empty;
        public string owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
        private Position _position;
        public Position position
        {
            get { return _position; }
            set { _position = value; }
        }
        private int _unitCount = 0;
        public int unitCount
        {
            get { return _unitCount; }
            set { _unitCount = value; }
        }

        public string type
        {
            get { return this.GetType().Name; }
        }

        public Tower ReturnSymmetricTower(int mapDimensionX, int mapDimensionY)
        {
            Tower symmetricTower = this.Clone();
            symmetricTower.position = this.position.SymmetricPosition(mapDimensionX, mapDimensionY);
            return symmetricTower;
        }

        public Tower Clone()
        {
            Tower newTower = (Tower)this.MemberwiseClone();
            newTower.owner = String.Copy(this.owner);
            newTower.position = new Position(this.position.X, this.position.Y);
            return newTower;
        }

        //private T Clone<T>(T obj)
        //{
        //    using (var stream = new MemoryStream())
        //    {
        //        var formatter = new BinaryFormatter();
        //        formatter.Serialize(stream, obj);
        //        stream.Position = 0;
        //        return (T)formatter.Deserialize(stream);
        //    };
        //}
    }
}