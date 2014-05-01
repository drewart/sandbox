using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LowestCombo
{
    class Feature { public string Name; public override string ToString() { return Name; } }

    class Plan { public string Name; public double Cost; public Feature[] Features; public override string ToString() { return Name; } } 

    
    /// <summary>
    /// Node class uses for searching plans
    /// </summary>
    class Node {

        public Plan Plan = null;
        public double Cost = 0.0;
        public List<Node> Children = new List<Node>();
        public List<Feature> NeededFeature;
        public Node Parent = null;

        /// <summary>
        /// constructor for root node
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="featuresNeeded"></param>
        public Node(Plan plan,Feature[] featuresNeeded)
        {
            Plan = plan;
            Cost = plan.Cost;
            NeededFeature = new List<Feature>(featuresNeeded);
            UpdateNeededFeatures(this);
        }

        /// <summary>
        /// constructor for child node
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="p"></param>
        public Node(Node parent, Plan plan)
        {
            this.Parent = parent;
            this.Plan = plan;
            this.Cost = Parent.Cost + plan.Cost;
            this.NeededFeature = new List<Feature>(parent.NeededFeature);

            UpdateNeededFeatures(this);
        }




        /// <summary>
        /// removes needed features based on current plan
        /// </summary>
        /// <param name="node">node to check which features are needed</param>
        public static void UpdateNeededFeatures(Node node)
        {
            foreach (Feature f in node.Plan.Features)
            {
                if (node.NeededFeature.Contains(f))
                    node.NeededFeature.Remove(f);

                if (node.NeededFeature.Count < 1)
                    return;
            }
        }
    }




    public class LowestCostPlans
    {




        Plan[] allPlans;                                                                   // this is the list of plans available instantiated as per the above

        Feature[] selectedFeatures;                                       // this is the list of features the user wants -> find combinations of 1-N plans that fulfill those features -> select the cheapest combination(s)


        //static Dictionary<string, List<Plan>> featurePlanHash = new Dictionary<string, List<Plan>>();

        static HashSet<string> PlanSeen = new HashSet<string>();
        //static Dictionary<string,bool> PlanSeen = new Dictionary<string,bool>();

        /// <summary>
        /// builds a dictionary of feature name and plans with that feature
        /// note prunes plans that the feature does nto exist.
        /// </summary>
        /// <param name="features"></param>
        /// <param name="plans"></param>
        /// <returns>dictionary of features and plans linked to that feature</returns>
        static Dictionary<string, List<Plan>> buildFeaturePlanHash(Feature[] features, Plan[] plans)
        {
            var featurePlanHash = new Dictionary<string, List<Plan>>();
           
            foreach (var plan in plans)
            {
                foreach (var feature in plan.Features)
                {
                    // skip features we don't care about
                    if (!(features.Where(f => f.Name == feature.Name)).Any())
                        continue;

                    if (featurePlanHash.ContainsKey(feature.Name))
                        featurePlanHash[feature.Name].Add(plan);
                    else
                        featurePlanHash.Add(feature.Name, new List<Plan> { plan });

                }
            }
            return featurePlanHash;
        }


        //static Dictionary<string, List<Plan>> featurePlanHash = null;

        /// <summary>
        /// finds the cheapest plan combination for the set of features
        /// </summary>
        /// <param name="features"></param>
        /// <param name="plans"></param>
        /// <returns>array of Plan(s) that contain the cheapest combination</returns>
        static Plan[] findCheapestPlansWithFeatures(Feature[] features, Plan[] plans)
        {
            PlanSeen.Clear();
            List<Plan> cheapestCombo = new List<Plan>();

            // costly but quicker feature lookup, also does pruning
            var featurePlanHash = buildFeaturePlanHash(features, plans);
           // if (featurePlanHash == null)
           //     featurePlanHash = buildPlanFeatureHash(features, plans);

            Stack<Node> nodes = new Stack<Node>();

            
            if (features.Length < 1)
            {
                throw new ArgumentNullException("features");
            }


            // get base cheapest by getting 
            foreach(Feature feature in features)
            {
            
                if (!featurePlanHash.ContainsKey(feature.Name))
                  throw new ArgumentException(string.Format("No Plan found with Feature {0}", feature.Name));

                List<Plan> lowestFeaturePlans = featurePlanHash[feature.Name].OrderBy(p => p.Cost).ToList();

                Plan cheapestFeaturePlan = lowestFeaturePlans.First<Plan>();

                if (!cheapestCombo.Contains(cheapestFeaturePlan))
                    cheapestCombo.Add(cheapestFeaturePlan);
                
            }

            // create nodes for every plan that contains a targeted feature
            foreach(List<Plan> featurePlans in featurePlanHash.Values)
                foreach(Plan featurePlan in featurePlans) 
                    nodes.Push(new Node(featurePlan, features));

            double lowestCost = cheapestCombo.Sum(p => p.Cost);

            // loop while no more nodes
            while(nodes.Any())
            {
                Node current = nodes.Pop();
                List<Node> results = SearchPlans(current, ref featurePlanHash, ref lowestCost);
                foreach (Node node in results)
                {
                    Plan[] currentPlans = GetPlansFromNode(node);
                    double currentCost = currentPlans.Sum(p => p.Cost);
                    if (currentCost < lowestCost)
                    {
                        lowestCost = currentCost;
                        cheapestCombo.Clear();
                        cheapestCombo.AddRange(currentPlans);

                    }
                }
                PlanSeen.Add(current.Plan.Name);
                
            }

            return cheapestCombo.ToArray();
        }


        /// <summary>
        /// recursive depth first search
        /// return a list of lowest cost plans from Tree
        /// expands child nodes for features needed and compares cost
        /// </summary>
        /// <param name="current"></param>
        /// <param name="featurePlans"></param>
        /// <param name="cost">cost</param>
        /// <returns></returns>
        static List<Node> SearchPlans(Node current, ref Dictionary<string, List<Plan>> featurePlans, ref double lowestCost)
        {
            List<Node> nodes = new List<Node>();
            // base case, if node doesn't need any more features and is less then current cost return node
            if (current.NeededFeature.Count == 0)
            {
                if (current.Cost < lowestCost)
                    nodes.Add(current);
                return nodes;
            }

            // get first needed feature
            Feature neadFeature = current.NeededFeature.First();
            
            // find plans with that feature
            foreach(Plan plan in featurePlans[neadFeature.Name].OrderBy(p => p.Cost))
            {
                // skip if plan already seen
                if (PlanSeen.Contains(plan.Name))
                    continue;

                // expand search if cost is less than current lowest cost
                if ((current.Cost + plan.Cost) > lowestCost)
                    continue;

                // remove features that this plan has
                List<Feature> neededRest = current.NeededFeature;

                Node child = new Node(current, plan);
                
                current.Children.Add(child);

                // Expand Nodes on child for needs
                var childNodes = SearchPlans(child, ref featurePlans, ref lowestCost);

                if (childNodes.Count > 0)
                    nodes.AddRange(childNodes);
            }
            return nodes;
        }

        /// <summary>
        /// get the Plan Path from a leaf node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        static Plan[] GetPlansFromNode(Node node)
        {
            Stack<Plan> plans = new Stack<Plan>();

            Node current = node;
            while (current != null)
            {
                plans.Push(current.Plan);
                current = current.Parent;
            }
            return plans.ToArray();
        }

        /// <summary>
        /// test driver 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Feature f1 = new Feature { Name = "f1"};
            Feature f2 = new Feature { Name = "f2"};
            Feature f3 = new Feature { Name = "f3"};
            Feature f4 = new Feature { Name = "f4"};
            Feature f5 = new Feature { Name = "f5"};
            Feature f6 = new Feature { Name = "f6"};




            Plan A = new Plan { Name = "A", Features = new Feature[] {f1} ,Cost = 2};
            Plan B = new Plan { Name = "B", Features = new Feature[] {f1,f2} ,Cost = 5};
            Plan C = new Plan { Name = "C", Features = new Feature[] {f3} ,Cost = 2};
            Plan D = new Plan { Name = "D", Features = new Feature[] {f2,f4} ,Cost = 3};
            Plan E = new Plan { Name = "E", Features = new Feature[] {f1,f4} ,Cost = 3};
            Plan F = new Plan { Name = "F", Features = new Feature[] {f1, f2 }, Cost = 2 };
            Plan G = new Plan { Name = "G", Features = new Feature[] {f1,f3 }, Cost = 3 };
            Plan H = new Plan { Name = "H", Features = new Feature[] {f2 }, Cost = 1 };
            Plan I = new Plan { Name = "I", Features = new Feature[] {f1, f2, f3 }, Cost = 2 };
            Plan J = new Plan { Name = "J", Features = new Feature[] {f5}, Cost = 2 };
            Plan K = new Plan { Name = "K", Features = new Feature[] {f6}, Cost = 2 };


            Plan[] plans = new Plan[] { A,B,C,D,E,F,G,H,I,J,K};

            Feature[] fA = new Feature[] { f1,f2, f3};

            Feature[] fB = new Feature[] { f1, f2 , f4};

            Feature[] fC = new Feature[] { f1,f4 };

            Feature[] fD = new Feature[] {  };

            Feature[] fE = new Feature[] { f1,f2,f3,f4,f5,f6 };


            Feature[][] featureSet = { fA,fB,fC,fD, fE};

            
                foreach(Feature[] features in featureSet)
                { 
                    try
                    {
               
                        Plan[] cheapestPlansFound = findCheapestPlansWithFeatures(features, plans);

                        if (cheapestPlansFound.Length > 0)
                        {
                            Console.WriteLine("Found cheapest Plan for features: {0}",features.Select(f => f.Name).Aggregate((a,b) => a + ", " + b));
                            Console.WriteLine("Plan(s): {0}", cheapestPlansFound.Select(p => p.Name).Aggregate((a, b) => a + ", " + b));
                            Console.WriteLine("Cost: {0}",cheapestPlansFound.Sum(p => p.Cost));
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
            }
            


        } // end main

       


    } // end class
}// end ns
