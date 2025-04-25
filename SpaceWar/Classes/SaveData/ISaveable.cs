using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace SpaceWar.Classes.SaveData
{
    public interface ISaveable
    {
        object SaveData(); // Возращает данные для сохранения
        void LoadData(object data, ContentManager content); // Загружает данные из сохранения   
    }
}
