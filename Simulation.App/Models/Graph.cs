using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.App.Models
{
    public class Graph
    {
        private Dictionary<Guid, Street> streets = new Dictionary<Guid, Street>();

        public void AddStreet(Street street)
        {
            streets[street.Id] = street;
        }

        // جستجو برای پیدا کردن مسیر
        public List<Street> FindRoute(Node startNode, Node goalNode)
        {
            // استفاده از جستجوی عمق اول (DFS) یا جستجوی عرض اول (BFS) برای پیدا کردن مسیر
            var route = new List<Street>();
            var visited = new HashSet<Guid>(); // برای جلوگیری از حلقه‌ها

            if (DFS(startNode, goalNode, visited, route))
            {
                return route;
            }

            return null; // اگر مسیری یافت نشد
        }

        // DFS برای جستجو
        private bool DFS(Node currentNode, Node goalNode, HashSet<Guid> visited, List<Street> route)
        {
            if (currentNode.Equals(goalNode))
                return true;

            foreach (var street in streets.Values)
            {
                if (street.Start.Equals(currentNode) && !visited.Contains(street.Id))
                {
                    visited.Add(street.Id);
                    route.Add(street);

                    if (DFS(street.End, goalNode, visited, route))
                        return true;

                    // بازگشت
                    route.RemoveAt(route.Count - 1);
                }
            }

            return false;
        }
    }

}
