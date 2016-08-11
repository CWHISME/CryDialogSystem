/**********************************************************
*Author: wangjiaying
*Date: 2016.8.11
*Func:
**********************************************************/

using System.IO;
using UnityEngine;

namespace CryDialog.Runtime
{
    public class Dialog : UniqueIDCalculator
    {
        /// <summary>
        /// 存储版本
        /// </summary>
        public const float SaveVersion = 0.1f;

        /// <summary>
        /// 保存数据，将会返回保存后的二进制数据
        /// </summary>
        public byte[] Save()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter w = new BinaryWriter(ms))
                {
                    w.Write(SaveVersion);

                    w.Write(JsonUtility.ToJson(this));
                    SaveThisNode(w);

                }
                return ms.ToArray();
            }

        }

        /// <summary>
        /// 从已保存的二进制数据中加载
        /// </summary>
        /// <param name="data"></param>
        public void Load(byte[] data)
        {
            if (data == null) return;
            if (data.Length > 0)
                using (MemoryStream ms = new MemoryStream(data))
                {
                    using (BinaryReader r = new BinaryReader(ms))
                    {
                        float ver = r.ReadSingle();
                        if (ver != SaveVersion)
                        {
                            Debug.LogError("Error:Archive data version not same! Curent: " + SaveVersion + " Data: " + ver);
#if UNITY_EDITOR
                            if (!UnityEditor.EditorUtility.DisplayDialog("Error!", "Error:Archive data version not the same! Curent Version: " + SaveVersion + " Data Version:" + ver, "Force Load", "Cancel"))
                                return;
#endif
                        }

                        string storyData = r.ReadString();
                        JsonUtility.FromJsonOverwrite(storyData, this);

                        LoadThisNode(r);
                    }
                }
        }

    }
}