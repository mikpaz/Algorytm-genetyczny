using System;

namespace AlgorytmGenetyczny
{
    class Program
    {
        static Random rnd = new Random();
        static void Main(string[] args)
        {
            start();
            Console.ReadLine();
        }

        public static void start()
        {
            Pop p;
            int pop = 100;
            p = new Pop(pop);

            int gen = 0;
            while (gen <= 10000)
            {
                p.Evolution(pop);
                ++gen;
            }
            Console.WriteLine("Rsult: " + p.bestFit(pop));
        }

        public static double getRandom(double min, double max)
        {
            return (rnd.NextDouble() * (max - min)) + min;

        }
        public class Topo
        {
            public int[] genes;

            public Topo()
            {
                this.genes = new int[2];
                for (int i = 0; i < genes.Length; i++)
                    this.genes[i] = (int)getRandom(-51.0, 51.0);
            }

            public void mutate()
            {
                for (int i = 0; i < genes.Length; i++)
                {
                    if (getRandom(0.0, 100) < 5)
                        this.genes[i] = (int)getRandom(-51.0, 51.0);
                }
            }
        }

        public class Pheno
        {
            double x, y;
            public Pheno(Topo gen)
            {
                this.x = gen.genes[0];
                this.y = gen.genes[1];
            }

            public double Value()
            {
                double value = 0;
                value = Math.Pow((Math.Sin(Math.Pow(x, 2)) + Math.Cos(Math.Pow(x, 2)) - 10), 2) + 5 * Math.Pow(x - y, 2);
                Console.WriteLine("[" + x + "; " + y + "], value: " + value);
                return value;
            }
        }


        public class Pick : IComparable<Pick>
        {
            public Topo topo;
            public Pheno pheno;
            public double fit;

            public void evaluate()
            {
                this.fit = pheno.Value();
            }

            public Pick()
            {
                this.topo = new Topo();
                this.pheno = new Pheno(topo);
                this.fit = 0.0;
            }



            int IComparable<Pick>.CompareTo(Pick obj)
            {
                Pick toCompare = (Pick)obj;
                if (fit < toCompare.fit)
                    return -1;
                else if (fit > toCompare.fit)
                    return 1;
                return 0;
            }
        }

        public static Pick New(Pick a, Pick b)
        {
            Pick c = new Pick();
            c.topo = Cross(a.topo, b.topo);
            c.topo.mutate();
            c.pheno = new Pheno(c.topo);
            return c;
        }

        static Topo Cross(Topo a, Topo b)
        {
            Topo c = new Topo();
            for (int i = 0; i < c.genes.Length; i++)
            {
                if (getRandom(0.0, 1) < 0.5)
                    c.genes[i] = a.genes[i];
                else
                    c.genes[i] = b.genes[i];
            }
            return c;
        }

        public class Pop
        {
            Pick[] pop;

            public double bestFit(int pNum)
            {
                double bestFit = new double();
                for (int i = 0; i < pNum; i++)
                {
                    if (i == 0)
                        bestFit = this.pop[i].fit;
                    else
                        if (this.pop[i].fit > bestFit)
                        bestFit = this.pop[i].fit;

                }
                return bestFit;
            }


            public void Evolution(int pCount)
            {
                Pick a = Pick(pCount);
                Pick b = Pick(pCount);
                Pick x = New(a, b);
                this.pop[0] = x;
                x.evaluate();
                Array.Sort(pop);
            }

            Pick Pick(int popNum)
            {
                int which = (int)Math.Floor((float)popNum * (1.0 - Math.Pow(getRandom(0.0, 1.0), 2)));
                return pop[which];
            }

            public Pop(int pNum)
            {
                this.pop = new Pick[pNum];

                for (int i = 0; i < pNum; i++)
                {
                    this.pop[i] = new Pick();
                    this.pop[i].evaluate();
                }
                Array.Sort(pop);
            }
        }
    }
}
