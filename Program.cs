using LINQ2;
using System.Linq;

namespace linq_learning
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            int totalHours =
               SampleData.Courses
            //.Sum(c => c.Hours);
            .Select(c => c.Hours)
            .Sum();                          // both right to call sum >> even call from num collection >> () , or call via any collection and send the num should be summed inside the ()

            Console.WriteLine(totalHours);


            var query =
                SampleData.Courses                // in query operator >> use .
                .Where(c => c.Hours > 30)
                .OrderByDescending(c => c.Name)  // order by >> return iordered >> has info >> by which it was ordered
                .ThenBy(c => c.Hours)
                .Select(c => new { c.Name, c.Hours });

            //from c in SampleData.Courses
            //where c.Hours > 30
            //orderby c.Name descending, c.Hours ascending
            //select new { c.Name, c.Hours };


            var queryy =
               from c in SampleData.Courses
               join s in SampleData.Subjects
               on c.Subject.Name equals s.Name
               select new { c.Name, Sub = s.Name };

            //SampleData.Courses.Join(SampleData.Subjects, c => c.subject.name,
            //    s => s.name,
            //    (c, s) => new { c.name, s.name });       // to be studied!!!!!!!!!!! query exp is enough no problem


            var query2 =
               //from c in SampleData.Courses
               //group c by c.Subject.Name;
               from c in SampleData.Courses               // compiler translation >> c is ienumerable??????? if get data from memory >> yes c is ien... >> if from d.b code is converted into sql
                                                          //group c by c.Subject.Name into grp 
               group c by c.Subject.Name; //into grp // result of group by into grp >> compiler make number of objects "depends on num of groups" from class implement igrouping "is a ienumerable + key"  , into enables u to continue your query after group by statment
               //select new   // if wanna uncomment this >> write above into grp >> u will change also in foreach
               //{
               //    SubName = grp.Key,
               //    TotalHours = grp.Sum(c => c.Hours),  // can use grp.select(c => c.hours).sum()  both are right 
               //    Count = grp.Count()
               //};


              foreach (IGrouping<string, Course> grp in query2)      // should be igrouping<string , course> instead of var  >> being seen as igrouping >> access via key , sum "allowed with any ienumerable >> igrouping is a ienumerable "????????  right
              {
                Console.WriteLine($"Subject: {grp.Key} \t TotalHours {grp.Sum(c => c.Hours)}");  
                foreach (var crs in grp)          // can iterate on grp here even if u received it above as var >> compiler detect that it is igrouping "which is ienumerable >> can iterate" as above u didnot make select so u didnot return ienumerable<anon obj> but u returned ienumerable<igrouping>
                {
                    Console.WriteLine($"Name: {crs.Name} \t Hours:{crs.Hours}");
                }
                Console.WriteLine("===================");
              }

            // prev ex >> if only for each on quer2 >> iterate enumerable of quer2 which is ienumerable<igrouping> >> each loop u have only one record from igrouping to be displayed >> so selection from igrouping here using anon obj must contain one row like key , aggreg func but u canot display details of each item inside igrouping itself "igrouping is a ienumerable so it has set of records in addition to its key"  , on the other hand if u applied for each also on each item inside the outer foreach >> u iterate on igrouping itself here so u can access data of each record separately >> can access details and data for each course inside igrouping as ex

            var query3 =
                  //from c in SampleData.Courses
                  //group c by c.Subject.Name;
                  from c in SampleData.Courses               
                                                             //group c by c.Subject.Name into grp 
                  group c by new { Sub = c.Subject.Name, Dept = c.Department.Name } into grp // result of group by into grp >> compiler make number of objects "depends on num of groups" from class implement igrouping "is a ienumerable + key"  , into enables u to continue your query after group by statment
                  select new   // if wanna uncomment this >> write above into grp >> u will change also in foreach
                  {
                      SubName = grp.Key,
                      TotalHours = grp.Sum(c => c.Hours),  // can use grp.select(c => c.hours).sum()  both are right 
                      Count = grp.Count()
                  };
            

            foreach (var grp in query3)      // should be var here >> using var >> only access via anon obj fields >> total hours , subName ....???????? right to be var here as u returned part from the igrouping obj above using anon obj
            {
                Console.WriteLine($"Subject: {grp.SubName} \t TotalHours {grp.TotalHours}");  
                //foreach (var crs in grp)  // cannot do this iteration as your query return ienumerable<anon> u made select above
                //{
                //    Console.WriteLine($"Name: {crs.Name} \t Hours:{crs.Hours}");
                //}
                Console.WriteLine("===================");
            }



        }
    }
}
