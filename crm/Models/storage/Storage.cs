using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.storage
{
    public class Storage<T> : IStorage<T>
    {

        #region vars
        T t;
        string path;
        Paths paths = Paths.getInstance();
        #endregion

        public Storage(string filename, T t)
        {            
            this.path = Path.Combine(paths.AppDir, filename);            
            this.t = t;
        }

        #region public
        public T load()
        {

            if (!File.Exists(path))
            {
                save(t);
            }

            string rd = File.ReadAllText(path);

            var p = JsonConvert.DeserializeObject<T>(rd);

            return p;


        }

        public void save(T p)
        {

            var json = JsonConvert.SerializeObject(p, Formatting.Indented);
            try
            {

                if (File.Exists(path))
                    File.Delete(path);

                File.WriteAllText(path, json);

            } catch (Exception ex)
            {
                throw new Exception("Не удалось сохранить файл JSON");
            }

        }
        #endregion
    }
}
