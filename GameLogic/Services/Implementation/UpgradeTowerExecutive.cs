using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic.TowerPackage.Factories;
using WeirdUnitBE.GameLogic.TowerPackage.Factories.ConcreteFactories;
using WeirdUnitBE.Middleware.JsonHandling;
using System.Text.RegularExpressions;

namespace WeirdUnitBE.GameLogic
{
    public class UpgradeTowerExecutive : IGameStateExecutable
    {
        public object ExecuteCommand(dynamic args, GameState gameState)
        {
            dynamic payload = args.jsonObj.payload;
            Position towerPosition = new Position((int)payload.position.X, (int)payload.position.Y);
            Tower upgradableTower = gameState.PositionToTowerDict[towerPosition];

            UpgradeTower(payload, ref upgradableTower);
            return FormatCommand(upgradableTower);
        }

        private static void UpgradeTower(dynamic payload, ref Tower tower)
        {
            string upgradeableTowerType = (string)payload.upgradeToType;
            Tower newTower = CreateEmptyTowerWithType(upgradeableTowerType);
            newTower.position = tower.position;
            newTower.unitCount = tower.unitCount;
            newTower.owner = tower.owner;
            newTower.neighbourTowers = tower.neighbourTowers;

            tower = newTower;
        }

        private static Tower CreateEmptyTowerWithType(string upgradableTowerType)
        {
            var strSplitByCamelCase = SplitCamelCase(upgradableTowerType).ToArray<string>();

            string concreteFactoryType = strSplitByCamelCase[0];
            AbstractFactory towerFactory = CreateTowerFactory(concreteFactoryType);

            string concreteTowerType = strSplitByCamelCase[1];
            Tower tower = towerFactory.CreateConcreteTower(concreteTowerType);
            
            return tower;
        }

        private static AbstractFactory CreateTowerFactory(string concreteFactoryType)
        {
            if (concreteFactoryType == "Default")
                return new DefaultTowerFactory();

            if (concreteFactoryType == "Fast")
                return new FastTowerFactory();
            
            return new StrongTowerFactory();
        }     

        private static object FormatCommand(Tower tower)
        {
            JsonMessageFormatterTemplate formatter = new JsonUpgradeTowerMessageFormatter();
            var buffer = formatter.FormatJsonBufferFromParams(tower);

            return buffer;
        }

        public static IEnumerable<string> SplitCamelCase(string source)
        {
            string pattern = UpperCaseCharRegex() + LowerCaseCharRegexRepeated();
            var matches = Regex.Matches(source, pattern);
            foreach (Match match in matches)
            {
                yield return match.Value;
            }
        }

        public static string UpperCaseCharRegex()
        {
            return @"[A-Z]";
        }

        public static string LowerCaseCharRegexRepeated()
        {
            return @"[a-z]*";
        }

    }
}

