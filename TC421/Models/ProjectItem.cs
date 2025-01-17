﻿namespace TC421
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;

    [Serializable]
    public class ProjectItem
    {
        private static ProjectItem _Item;
        private string _ProjectPath;
        private Dictionary<string, ModelItem> _ModelSet;

        public string ProjectPath
        {
            set => this._ProjectPath = value;
            get => this._ProjectPath ?? (this._ProjectPath = this.GetSettingsFilePath());
        }

        public string ProjectName { set; get; }

        public Dictionary<string, ModelItem> ModelSet
        {
            set => this._ModelSet = value;
            get => this._ModelSet ?? (this._ModelSet = new Dictionary<string, ModelItem>());
        }

        public static ProjectItem Instance
        {
            get => ProjectItem._Item ?? (ProjectItem._Item = new ProjectItem());
            set => ProjectItem._Item = value;
        }

        private string GetSettingsFilePath() => new DirectoryInfo(Environment.CurrentDirectory) + Path.DirectorySeparatorChar.ToString();

        public bool Save(string filename = "profile.json", bool IsCreat = false)
        {
            if (IsCreat && File.Exists(filename))
                return false;

            using (FileStream serializationStream = new FileStream(filename, FileMode.Create))
            {
                byte[] bytesA = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject((ProjectItem)this, Formatting.Indented));
                serializationStream.Write(bytesA, 0, bytesA.Length);
                return true;
            }
        }

        public ProjectItem Open(string filename = null)
        {
            try
            {
                if (string.IsNullOrEmpty(filename)) filename = this.ProjectPath + "profile.json";
                ProjectItem._Item = JsonConvert.DeserializeObject<ProjectItem>(Encoding.UTF8.GetString(File.ReadAllBytes(filename)), new JsonSerializerSettings
                {
                    ObjectCreationHandling = ObjectCreationHandling.Replace
                });
                return ProjectItem._Item;
            }
            catch(Exception ex) { }

            return null;
        }
    }
}
