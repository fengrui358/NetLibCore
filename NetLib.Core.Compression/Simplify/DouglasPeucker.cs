using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace FrHello.NetLib.Core.Compression.Simplify
{
    /// <summary>
    /// 参考：http://www.kangry.net/blog/?type=article&article_id=452
    /// https://github.com/IrisLoveLeo/DouglasPeucker
    /// </summary>
    public class DouglasPeucker
    {
        private static readonly Lazy<DouglasPeucker> _instance = new Lazy<DouglasPeucker>(() => new DouglasPeucker());

        /// <summary>
        /// 实例
        /// </summary>
        public static DouglasPeucker Instance => _instance.Value;

        private DouglasPeucker()
        {
        }

        /// <summary>
        /// 抽稀
        /// </summary>
        /// <param name="pointsList">需要精简的点</param>
        /// <param name="errorBound">误差范围</param>
        /// <returns>返回精简后的点</returns>
        public IEnumerable<PointF> Simplify(IEnumerable<PointF> pointsList, double errorBound)
        {
            var innerPointsList = new List<PointF>();
            if (pointsList != null)
            {
                var pointFs = pointsList as PointF[] ?? pointsList.ToArray();
                var last = pointFs.First();
                innerPointsList.Add(last);

                for (int i = 1; i < pointFs.Length; i++)
                {
                    //Discard the same points
                    if (!last.Equals(pointFs[i]))
                    {
                        innerPointsList.Add(pointFs[i]);
                        last = pointFs[i];
                    }
                }
            }

            var result = CompressHelper(innerPointsList, errorBound);

            if (result.Last() != innerPointsList.Last())
            {
                result.Add(innerPointsList.Last());
            }

            return result;
        }

        /// <summary>
        /// 抽稀
        /// </summary>
        /// <param name="pointsList">需要精简的点</param>
        /// <param name="errorBound">误差范围</param>
        /// <returns>返回精简后的点</returns>
        public IEnumerable<Point> Simplify(IEnumerable<Point> pointsList, double errorBound)
        {
            var innerPointsList = new List<Point>();
            if (pointsList != null)
            {
                var pointFs = pointsList as Point[] ?? pointsList.ToArray();
                var last = pointFs.First();
                innerPointsList.Add(last);

                for (int i = 1; i < pointFs.Length; i++)
                {
                    //Discard the same points
                    if (!last.Equals(pointFs[i]))
                    {
                        innerPointsList.Add(pointFs[i]);
                        last = pointFs[i];
                    }
                }
            }

            var result = CompressHelper(innerPointsList, errorBound);

            if (result.Last() != innerPointsList.Last())
            {
                result.Add(innerPointsList.Last());
            }

            return result;
        }

        /// <summary>
        /// 抽稀
        /// </summary>
        /// <param name="pointsList">需要精简的点</param>
        /// <param name="errorBound">误差范围</param>
        /// <returns>返回精简后的点</returns>
        public IEnumerable<BaseModels.Point> Simplify(IEnumerable<BaseModels.Point> pointsList, double errorBound)
        {
            var innerPointsList = new List<BaseModels.Point>();
            if (pointsList != null)
            {
                var pointFs = pointsList as BaseModels.Point[] ?? pointsList.ToArray();
                var last = pointFs.First();
                innerPointsList.Add(last);

                for (int i = 1; i < pointFs.Length; i++)
                {
                    //Discard the same points
                    if (!last.Equals(pointFs[i]))
                    {
                        innerPointsList.Add(pointFs[i]);
                        last = pointFs[i];
                    }
                }
            }

            var result = CompressHelper(innerPointsList, errorBound);

            if (result.Last() != innerPointsList.Last())
            {
                result.Add(innerPointsList.Last());
            }

            return result;
        }

        private List<Point> CompressHelper(List<Point> pointsList, double errorBound)
        {
            if (pointsList.Count < 2)
            {
                return pointsList;
            }

            var result = new List<Point>();

            // 有可能是polygon
            if (pointsList.First().Equals(pointsList.Last()))
            {
                var r1 = CompressHelper(pointsList.GetRange(0, pointsList.Count / 2), errorBound);
                var r2 = CompressHelper(
                    pointsList.GetRange(pointsList.Count / 2, pointsList.Count - pointsList.Count / 2), errorBound);
                result.AddRange(r1);
                result.AddRange(r2);
                return result;
            }

            var line = new Line(pointsList.First(), pointsList.Last());

            double maxDistance = 0;
            int maxIndex = 0;

            for (int i = 1; i < pointsList.Count - 1; i++)
            {
                var distance = Distance(pointsList[i], line);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    maxIndex = i;
                }
            }

            if (maxDistance <= errorBound)
            {
                result.Add(pointsList.First());
            }
            else
            {
                var r1 = CompressHelper(pointsList.GetRange(0, maxIndex), errorBound);
                var r2 = CompressHelper(pointsList.GetRange(maxIndex + 1, pointsList.Count - maxIndex - 1), errorBound);
                result.AddRange(r1);
                result.Add(pointsList[maxIndex]);
                result.AddRange(r2);
            }

            return result;
        }

        private List<BaseModels.Point> CompressHelper(List<BaseModels.Point> pointsList, double errorBound)
        {
            if (pointsList.Count < 2)
            {
                return pointsList;
            }

            var result = new List<BaseModels.Point>();

            // 有可能是polygon
            if (pointsList.First().Equals(pointsList.Last()))
            {
                var r1 = CompressHelper(pointsList.GetRange(0, pointsList.Count / 2), errorBound);
                var r2 = CompressHelper(
                    pointsList.GetRange(pointsList.Count / 2, pointsList.Count - pointsList.Count / 2), errorBound);
                result.AddRange(r1);
                result.AddRange(r2);
                return result;
            }

            var line = new LineD(pointsList.First(), pointsList.Last());

            double maxDistance = 0;
            int maxIndex = 0;

            for (int i = 1; i < pointsList.Count - 1; i++)
            {
                var distance = Distance(pointsList[i], line);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    maxIndex = i;
                }
            }

            if (maxDistance <= errorBound)
            {
                result.Add(pointsList.First());
            }
            else
            {
                var r1 = CompressHelper(pointsList.GetRange(0, maxIndex), errorBound);
                var r2 = CompressHelper(pointsList.GetRange(maxIndex + 1, pointsList.Count - maxIndex - 1), errorBound);
                result.AddRange(r1);
                result.Add(pointsList[maxIndex]);
                result.AddRange(r2);
            }

            return result;
        }

        private List<PointF> CompressHelper(List<PointF> pointsList, double errorBound)
        {
            if (pointsList.Count < 2)
            {
                return pointsList;
            }

            var result = new List<PointF>();

            // 有可能是polygon
            if (pointsList.First().Equals(pointsList.Last()))
            {
                var r1 = CompressHelper(pointsList.GetRange(0, pointsList.Count / 2), errorBound);
                var r2 = CompressHelper(
                    pointsList.GetRange(pointsList.Count / 2, pointsList.Count - pointsList.Count / 2), errorBound);
                result.AddRange(r1);
                result.AddRange(r2);
                return result;
            }

            var line = new LineF(pointsList.First(), pointsList.Last());

            double maxDistance = 0;
            int maxIndex = 0;

            for (int i = 1; i < pointsList.Count - 1; i++)
            {
                var distance = Distance(pointsList[i], line);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    maxIndex = i;
                }
            }

            if (maxDistance <= errorBound)
            {
                result.Add(pointsList.First());
            }
            else
            {
                var r1 = CompressHelper(pointsList.GetRange(0, maxIndex), errorBound);
                var r2 = CompressHelper(pointsList.GetRange(maxIndex + 1, pointsList.Count - maxIndex - 1), errorBound);
                result.AddRange(r1);
                result.Add(pointsList[maxIndex]);
                result.AddRange(r2);
            }

            return result;
        }

        private double Distance(Point p, Line line)
        {
            var p1 = line.Point1;
            var p2 = line.Point2;
            return Math.Abs(
                    ((p2.Y - p1.Y) * p.X + (p1.X - p2.X) * p.Y + (p1.Y - p2.Y) * p1.X + (p2.X - p1.X) * p1.Y) /
                    Math.Sqrt((p2.Y - p1.Y) * (p2.Y - p1.Y) + (p1.X - p2.X) * (p1.X - p2.X))
                );
        }

        private double Distance(PointF p, LineF line)
        {
            var p1 = line.Point1;
            var p2 = line.Point2;
            return Math.Abs(
                ((p2.Y - p1.Y) * p.X + (p1.X - p2.X) * p.Y + (p1.Y - p2.Y) * p1.X + (p2.X - p1.X) * p1.Y) /
                Math.Sqrt((p2.Y - p1.Y) * (p2.Y - p1.Y) + (p1.X - p2.X) * (p1.X - p2.X))
            );
        }

        private double Distance(BaseModels.Point p, LineD line)
        {
            var p1 = line.Point1;
            var p2 = line.Point2;
            return Math.Abs(
                ((p2.Y - p1.Y) * p.X + (p1.X - p2.X) * p.Y + (p1.Y - p2.Y) * p1.X + (p2.X - p1.X) * p1.Y) /
                Math.Sqrt((p2.Y - p1.Y) * (p2.Y - p1.Y) + (p1.X - p2.X) * (p1.X - p2.X))
            );
        }
    }

    class Line
    {
        public Point Point1 { get; }

        public Point Point2 { get; }

        public Line(Point point1, Point point2)
        {
            Point1 = point1;
            Point2 = point2;
        }
    }

    class LineF
    {
        public PointF Point1 { get; }

        public PointF Point2 { get; }

        public LineF(PointF point1, PointF point2)
        {
            Point1 = point1;
            Point2 = point2;
        }
    }

    class LineD
    {
        public BaseModels.Point Point1 { get; }

        public BaseModels.Point Point2 { get; }

        public LineD(BaseModels.Point point1, BaseModels.Point point2)
        {
            Point1 = point1;
            Point2 = point2;
        }
    }
}