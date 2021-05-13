using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQExercise
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Question1();
            Question2();
            Question3();
            Question4();
            Question5();
            Question6();
            Question7();
            Question8();
            Question9();
            Question10();
            Question11();
            Question12();
            Question13();
        }

        /*Question 1:
            Take the following string "Davis, Clyne, Fonte, Hooiveld, Shaw, Davis, Schneiderlin, Cork, Lallana, Rodriguez, Lambert" 
            and give each player a shirt number, starting from 1, to create a string of the form: "1. Davis, 2. Clyne, 3. Fonte" etc
        */
        public static void Question1()
        {
            String str = "Davis, Clyne, Fonte, Hooiveld, Shaw, Davis, Schneiderlin, Cork, Lallana, Rodriguez, Lambert";

            var newStr = str.Split(',')
                    .Select((name, index) => (index + 1).ToString() + ". " + name.Trim());

            var combine = String.Join(", ", newStr);

            Console.WriteLine("\nQuestion 1:");
            Console.WriteLine(combine);
        }



        /*Question 2:
           Take the following string "Jason Puncheon, 26/06/1986; Jos Hooiveld, 22/04/1983; Kelvin Davis, 29/09/1976; 
           Luke Shaw, 12/07/1995; Gaston Ramirez, 02/12/1990; Adam Lallana, 10/05/1988" and turn it into an IEnumerable 
           of players in order of age (bonus to show the age in the output)
        */
        public static void Question2()
        {
            var str = "Jason Puncheon, 26/06/1986; Jos Hooiveld, 22/04/1983; Kelvin Davis, 29/09/1976; Luke Shaw, 12/07/1995; Gaston Ramirez, 02/12/1990; Adam Lallana, 10/05/1988";

            var newStr = str.Split(';')
                .Select((nameAndDate) => nameAndDate.Split(','))
                .Select(texts => (name: texts[0].Trim(), age: DateTime.Now - DateTime.Parse(texts[1])))
                .OrderBy(tuple => tuple.age);

            Console.WriteLine("\nQuestion 2:");

            foreach (var item in newStr)
            {
                Console.WriteLine(item.name + ", " + "{0:0.00}", item.age.TotalDays / 365);
            }
        }

        /*Question 3:
            Take the following string "4:12,2:43,3:51,4:29,3:24,3:14,4:46,3:25,4:52,3:27" which represents the 
            durations of songs in minutes and seconds, and calculate the total duration of the whole album
        */
        public static void Question3()
        {
            var str = "4:12,2:43,3:51,4:29,3:24,3:14,4:46,3:25,4:52,3:27";

            var totalTime = str.Split(',')
                .Select(time => TimeSpan.Parse(time))
                .Aggregate((time1, time2) => time1 + time2);

            Console.WriteLine("\nQuestion 3:");
            Console.WriteLine("The total time is: " + totalTime.Minutes + " minutes, and " + totalTime.Seconds + " seconds");
        }

        /*Question 4:
            Create an enumerable sequence of strings in the form "x,y" representing all the points on a 3x3 grid. 
            e.g. output would be: 0,0 0,1 0,2 1,0 1,1 1,2 2,0 2,1 2,2
        */
        public static void Question4()
        {
            int limit = 3;

            var pairs = Enumerable.Range(0, limit)
                .SelectMany(x => Enumerable.Range(0, limit).Select(y => (x: x, y: y)))
                .Select(pair => $"[{pair.x}, {pair.y}]"); ;

            var res = string.Join(", ", pairs);

            Console.WriteLine("\nQuestion 4:");
            Console.WriteLine(res);
        }

        /*Question 5:
            Take the following string  which represents the times 
            (in minutes and seconds) at which a swimmer completed each of 10 lengths. Turn this into an enumerable of timespan 
            objects containing the time taken to swim each length (e.g. first length was 45 seconds, second was 47 seconds etc)*/

        public static void Question5()
        {
            var source = "00:45,01:32,02:18,03:01,03:44,04:31,05:19,06:01,06:47,07:35";

            var swimmerTime = $"00:00,{source}"
                .Split(',')
                .Select(str => TimeSpan.Parse($"00:{str.Trim()}"))
                .window(2)
                .Select(pair => pair[1] - pair[0]);

            Console.WriteLine("\nQuestion 5:");

            foreach (var item in swimmerTime)
            {
                Console.WriteLine(item);
            }
        }

        public static IEnumerable<T[]> window<T>(this IEnumerable<T> source, int count)
        {
            var queue = new Queue<T>();

            foreach (var item in source)
            {
                queue.Enqueue(item);

                if (queue.Count > count)
                {
                    queue.Dequeue();
                }

                if (queue.Count == count)
                {
                    yield return queue.ToArray();
                }
            }
        }

        /*Question 6:
         * Take the following string "2,5,7-10,11,17-18" and turn it into an IEnumerable of integers: 2 5 7 8 9 10 11 17 18*/
        public static void Question6()
        {
            //Not finished
            var source = "2,5,7-10,11,17-18";

            var splited = source
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(str => str
                    .Split("-", StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.Parse(s))
                    .ToArray())
                .SelectMany(items => items.Length == 1
                            ? Enumerable.Repeat(items[0], 1)
                            : Enumerable.Range(items[0], items[1] - items[0] + 1));

            Console.WriteLine("\nQuestion 6:");

            foreach (var item in splited)
            {
                Console.WriteLine(item);
            }
        }

        /*Question 7:
         * In a motor sport competition, a player's points total for the season is the sum of all the points earned in each race,
         * but the three worst results are not counted towards the total. Calculate the following player's score based on the points earned in each round:
         * "10,5,0,8,10,1,4,0,10,1"*/
        public static void Question7()
        {
            var src = "10,5,0,8,10,1,4,0,10,1";

            var res = src.Split(',')
                .Select(s => int.Parse(s))
                .OrderBy(n => n)
                .Skip(3)
                .Sum();

            Console.WriteLine("\nQuestion 7:");

            Console.WriteLine(res);
        }

        /*Question 8:
         * A chess board is an 8x8 grid, from a1 in the bottom left to h8 in the top right.
         * A bishop can travel diagonally any number of squares. So for example a bishop on c5 can go to b4 or to a3 in one move.
         * Create an enumerable sequence of board positions that includes every square a bishop can move to in one move on an empty chess board,
         * if its starting position is c6. e.g. output would include b7, a8, b5, a4*/
        public static void Question8()
        {
            var source = ('c', 6);

            var oneToEight = Enumerable.Range(1, 8);

            var upLeft = oneToEight
                .Select(i => ((char)(source.Item1 - i), source.Item2 - i))
                .TakeWhile(pair => (pair.Item1 >= 'a') && (pair.Item2 >= 1));

            var upRight = oneToEight
                .Select(i => ((char)(source.Item1 - i), source.Item2 + i))
                .TakeWhile(pair => (pair.Item1 >= 'a') && (pair.Item2 <= 8));

            var downLeft = oneToEight
                .Select(i => ((char)(source.Item1 + i), source.Item2 - i))
                .TakeWhile(pair => (pair.Item1 <= 'h') && (pair.Item2 >= 1));

            var downRight = oneToEight
                .Select(i => ((char)(source.Item1 + i), source.Item2 + i))
                .TakeWhile(pair => (pair.Item1 <= 'h') && (pair.Item2 <= 8));

            var res = upLeft
                .Concat(upRight)
                .Concat(downLeft)
                .Concat(downRight)
                .Select(pair => $"{pair.Item1}{pair.Item2}");

            Console.WriteLine("Question 8");
            Console.WriteLine(string.Join(" ", res));
        }

        /*Question 9:
         * The following sequence has 100 entries. Sample it by taking every 5th value and discarding the rest.
         * The output should begin with 24,53,77,...*/
        public static void Question9()
        {
            var src = "0,6,12,18,24,30,36,42,48,53,58,63,68,72,77,80,84,87,90,92,95,96,98,99,99" +
                      ",100,99,99,98,96,95,92,90,87,84,80,77,72,68,63,58,53,48,42,36,30,24,18,12" +
                      ",6,0,-6,-12,-18,-24,-30,-36,-42,-48,-53,-58,-63,-68,-72,-77,-80,-84,-87" +
                      ",-90,-92,-95,-96,-98,-99,-99,-100,-99,-99,-98,-96,-95,-92,-90,-87,-84,-80" +
                      ",-77,-72,-68,-63,-58,-53,-48,-42,-36,-30,-24,-18,-12,-6";

            var res = src.Split(',')
                .Select((val, index) => (val: val, index: index + 1))
                .Where(n => n.index % 5 == 0)
                .Select(t => t.val); ;

            var fin = string.Join(", ", res);

            Console.WriteLine("Question 9:");
            Console.WriteLine(fin);
        }

        /*Question 10:
         * Yes won the vote, but how many more Yes votes were there than No votes?
         * "Yes,Yes,No,Yes,No,Yes,No,No,No,Yes,Yes,Yes,Yes,No,Yes,No,No,Yes,Yes"*/
        public static void Question10()
        {
            var src = "Yes,Yes,No,Yes,No,Yes,No,No,No,Yes,Yes,Yes,Yes,No,Yes,No,No,Yes,Yes";

            var res = src
                .Split(',')
                .Sum(s => s.Trim() == "Yes" ? 1 : -1);

            Console.WriteLine("Question 10:");
            Console.WriteLine(res);
        }

        /*Question 11:
         * Count how many have dogs, how many have cats, and how many have other pets.
         * e.g. output would be a structure or sequence containing: 
         * Dog:5 Cat:3 Other:4 
         * "Dog,Cat,Rabbit,Dog,Dog,Lizard,Cat,Cat,Dog,Rabbit,Guinea Pig,Dog"*/
        public static void Question11()
        {
            var src = "Dog,Cat,Rabbit,Dog,Dog,Lizard,Cat,Cat,Dog,Rabbit,Guinea Pig,Dog";

            var res = src.Split(',')
                .GroupBy(s => s == "Dog" || s == "Cat" ? s : "Other")
                .ToDictionary(g => g.Key, g => g.Count());

            Console.WriteLine("\nQuestion 11:");
            foreach (var item in res)
            {
                Console.WriteLine(item);
            }
        }

        /*Question 12:
         * The following string contains number of sales made per day in a month:
            "1,2,1,1,0,3,1,0,0,2,4,1,0,0,0,0,2,1,0,3,1,0,0,0,6,1,3,0,0,0"
            How long is the longest sequence of days without a sale? (in this example it's 4)*/
        public static void Question12()
        {
            var str = "1,2,1,1,0,3,1,0,0,2,4,1,0,0,0,0,2,1,0,3,1,0,0,0,6,1,3,0,0,0";

            var res = str
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Aggregate((current: 0, max: 0),
                           (acc, item) => item != "0" 
                           ? (current: 0, max: acc.max)
                           : (current: acc.current + 1, max: Math.Max(acc.max, acc.current + 1)),
                            acc => acc.max);

            Console.WriteLine("\nQuestion 12:");
            Console.WriteLine(res);
        }

        /* Question 13:
         From the following list of names 
         "Santi Cazorla, Per Mertesacker, Alan Smith, Thierry Henry, Alex Song, Paul Merson, Alexis Sánchez, Robert Pires, Dennis Bergkamp, Sol Campbell"
         find any groups of people who share the same initials as each other.*/
        public static void Question13()
        {
            var src = "Santi Cazorla, Per Mertesacker, Alan Smith, Thierry Henry, Alex Song, Paul Merson," +
                      " Alexis Sánchez, Robert Pires, Dennis Bergkamp, Sol Campbell";

            var res = src
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Select(splitName)
                .Select(s => (first: s.first, last: s.last, inits: (s.first[0], s.last[0])))
                .GroupBy(s => s.inits);

            Console.WriteLine("\nQuestion 13:");

            foreach (var item in res)
            {
                Console.WriteLine($"{item.Key}:");

                foreach (var person in item)
                {
                    Console.WriteLine($"{person.first} {person.last}");
                }

                Console.WriteLine();
            }
        }

        private static (string first, string last) splitName(String s)
        {
            var names = s.Split(' ');

            return (names[0], names[1]);
        }
    }
}
