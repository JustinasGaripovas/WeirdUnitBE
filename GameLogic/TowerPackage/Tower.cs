using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using WeirdUnitBE.GameLogic;
using System.Reflection;

namespace WeirdUnitBE.GameLogic.TowerPackage
{
    [Serializable]
    public abstract class Tower
    {
        private Position _position;
        public Position position
        {
            get { return _position; }
            set { _position = value; }
        }
        public int _unitCount = 0;
        public int unitCount
        {
            get { return _unitCount; }
            set { _unitCount = value; }
        }
        public string type;

        public string _type = this.GetType().Name;

        public Tower ReturnSymmetricTower(int mapDimensionX, int mapDimensionY)
        {
            Tower symmetricTower = this.Clone<Tower>(this);
            symmetricTower.position = this.position.InvertPosition(mapDimensionX, mapDimensionY);
            return symmetricTower;
        }
        private T Clone<T>(T obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            };
        }
    }
}