using System;
using LitJson;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace JSON4DB
{
	public class JSON4DB<T>
	{
        /// <summary>
        /// 当前实例
        /// </summary>
        private T tatget;

		/// <summary>
		/// 数据库路径
		/// </summary>
		private string dataPath;

		/// <summary>
		/// 数据字典
		/// </summary>
		private Dictionary<string,T> dicts;

		/// <summary>
		/// 构造函数
		/// </summary>
		private JSON4DB()
		{
            this.tatget = CreateInitiate();
            this.dicts = new Dictionary<string, T>();
		}

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataPath">数据库路径</param>
        private JSON4DB(string dataPath)
        {
            this.tatget = CreateInitiate();
            this.dataPath = dataPath;
            this.dicts =ReadJSON();
        }

        /// <summary>
        /// 创建数据库,如果文件存在则覆盖
        /// </summary>
        /// <param name="dataPath">数据库路径</param>
        /// <returns></returns>
        public static JSON4DB<T> Create(string dataPath)
        {
            JSON4DB<T> db = new JSON4DB<T>();
            db.dataPath = dataPath;
            db.Commit();
            return db;
        }

        /// <summary>
        /// 读取数据库，如果文件不存在则引发异常
        /// </summary>
        /// <param name="dataPath"></param>
        /// <returns></returns>
        public static JSON4DB<T> Load(string dataPath)
        {
            JSON4DB<T> db = new JSON4DB<T>(dataPath);
            return db;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="id">唯一ID</param>
        /// <param name="t">数据</param>
		public void Insert(string id,T t)
		{
			if(dicts == null || dicts.ContainsKey(id))
				return;
			
			//增加元素
			dicts.Add(id, t);
		}

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">唯一ID</param>
		public void Delete(string id)
		{
            if(dicts == null || !dicts.ContainsKey(id))
				return;

			//移除元素
			dicts.Remove(id);
		}

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="id">唯一ID</param>
        /// <returns></returns>
		public T Read(string id)
		{
			if(dicts == null || !dicts.ContainsKey(id))
				return default(T);
			
			return dicts[id];
		}

        /// <summary>
        ///  读取数据
        /// </summary>
        /// <returns></returns>
		public T[] Read()
		{
            if(dicts == null)
                return null;

            //创建数组
            T[] data = new T[dicts.Keys.Count];

            //数组索引
            int index = 0;
            
            //遍历字典
            foreach (KeyValuePair<string, T> kv in dicts)
            {
                data[index] = kv.Value;
                index += 1;
            }

            return data;
		}

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        public List<T> ReadList()
		{
            if(dicts == null)
                return null;

            //创建列表
            List<T> data = new List<T>();
            
            //遍历字典
            foreach (KeyValuePair<string, T> kv in dicts)
            {
                data.Add(kv.Value);
            }

            return data;
		}

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="t"></param>
		public void Update(string id,T t)
		{
            if(dicts == null || !dicts.ContainsKey(id))
                return;

            dicts[id] = t;
		}

        /// <summary>
        /// 提交更改
        /// </summary>
		public void Commit()
		{
            if(dataPath == null)
                return;

            WriteJSON();
		}

        /// <summary>
        /// 创建当前类型实例
        /// </summary>
        /// <returns>The initiate.</returns>
        private T CreateInitiate()
        {
            Type t = typeof(T);
            ConstructorInfo ct = t.GetConstructor(System.Type.EmptyTypes);
            return (T)ct.Invoke(null);
        }

        /// <summary>
        /// 读取JSON数据并返回数据字典
        /// </summary>
        /// <returns></returns>
        private Dictionary<string,T> ReadJSON()
        {
            Dictionary<string, T> data = null;

            if(!File.Exists(dataPath))
                throw new FileNotFoundException("请确认数据文件是否存在!");

            if(dataPath != null && File.Exists(dataPath))
            {
                //读取数据
                StreamReader streamReader = new StreamReader(dataPath);
                string json = streamReader.ReadToEnd();
                streamReader.Close();
                //反序列化
                data = JsonMapper.ToObject<Dictionary<string, T>>(json);
            }
            return data;
        }

        /// <summary>
        /// 写入JSON数据至文本
        /// </summary>
        private void WriteJSON()
        {
            if(dataPath == null)
                return;

            //写入JSON数据
            string data = JsonMapper.ToJson(dicts);
            StreamWriter streamWriter = new StreamWriter(dataPath, false, Encoding.UTF8);
            streamWriter.Write(data);
            streamWriter.Close();
        }
	}
}

