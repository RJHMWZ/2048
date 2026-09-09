using System.Collections.Generic;

/// <summary>
/// 2048单行移动与合并工具
/// </summary>
/// <summary>
/// 2048单行移动与合并工具
/// </summary>
public static class LineMergeUtility
{
    /// <summary>
    /// 将一行数字向左压缩并合并
    /// </summary>
    /// <param name="line">原始行数据</param>
    /// <returns>合并结果</returns>
    public static LineMergeResult MergeLeft(int[] line)
    {
        if (line == null)
        {
            return new LineMergeResult(null, 0);
        }

        List<int> numbers = new List<int>();

        //第一步：移除所有0
        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] != 0)
            {
                numbers.Add(line[i]);
            }
        }

        int score = 0;

        //第二步：合并相邻相同数字
        for (int i = 0; i < numbers.Count - 1; i++)
        {
            if (numbers[i] == numbers[i + 1])
            {
                //合并
                numbers[i] *= 2;

                //本次合并后的数字就是获得的分数
                score += numbers[i];

                //删除被合并掉的数字
                numbers.RemoveAt(i + 1);
            }
        }

        //第三步：补0
        int[] result = new int[line.Length];

        for (int i = 0; i < numbers.Count; i++)
        {
            result[i] = numbers[i];
        }

        return new LineMergeResult(result, score);
    }
}