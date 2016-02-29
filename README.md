# JSON4DB
JSON4DB是一个基于JSON的轻量级数据库，它使用JSON来作为数据的组织形式，是一个面向对象的轻量级数据库。
该数据库使得开发者在编写数据库应用的时候，无需再去关注SQL语句，因为在这里一切都是对象，从数据库到数据，
程序员完全可以用它最为熟悉的面向对象编程，我编写这样一个项目的初衷就是让数据库变得更为简单，在我们希望
存储简单的数据结构的时候，这个项目能够让我们从SQLServer、MySQL等这些复杂的数据库中解脱出来，可能这个项目
无法承载“大数据”的需求，然而对我来说，简单到好用这就足够了！

#代码示例
```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSON4DB;

namespace JSON4DBSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建数据库
            JSON4DB<Student> db = JSON4DB<Student>.Create("test.json");
            //插入一条数据
            db.Insert("001", new Student(001, "张三", 12));
            //提交更改
            db.Commit();

            //加载数据库
            db = JSON4DB<Student>.Load("test.json");
            //删除ID为001的记录
            db.Delete("001");

            //插入三条数据
            db.Insert("001", new Student(002, "张三", 12));
            db.Insert("002", new Student(003, "李四", 20));
            db.Insert("003", new Student(004, "王五", 25));

            //更新ID为003的记录
            db.Update("003", new Student(001, "Wang5", 15));

            //提交更改
            db.Commit();

            //读取ID为002的记录
            Student stu = db.Read("002");
            
            //读取所有记录返回一个列表
            List<Student> studentList = db.ReadList();

            //读取所有记录返回一个数组
            Student[] students = db.Read();

            Console.WriteLine(students[0].ToString());
            Console.ReadKey();
        }
    }

    /// <summary>
    /// 定义一个示例类Student
    /// </summary>
    public class Student
    {
        public Student()
        {

        }

        public Student(int mID, string mName, int mAge)
        {
            this.mID = mID;
            this.mName = mName;
            this.mAge = mAge;
        }

        private int mAge;
        public int Age
        {
            get { return mAge; }
            set { mAge = value; }
        }

        private int mID;
        public int ID
        {
            get { return mID; }
            set { mID = value; }
        }

        private string mName;
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }

        public override string ToString()
        {
            return string.Format("ID={0},Name={1},Age={2}", this.ID, this.Name, this.Age);
        }
    }
}
```

