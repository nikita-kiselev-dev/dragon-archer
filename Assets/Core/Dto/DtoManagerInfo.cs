﻿using System.IO;
using UnityEngine;

namespace Core.Dto
{
    public class DtoManagerInfo
    {
        public const string ConfigName = "Config.bin";
        public static readonly string ConfigDirectory = $"{Application.persistentDataPath}/Data/";
        public static readonly string ConfigPath = Path.Combine(ConfigDirectory, ConfigName);
    }
}