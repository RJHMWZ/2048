using System.Collections.Generic;

/// <summary>
/// 2048单行移动与合并工具
/// </summary>
public static class LineMergeUtility
{
    /// <summary>
    /// 将一行数字向左压缩并合并
    /// </summary>
    /// <param name="line">原始行数据</param>
    /// <returns>合并后的新行数据</returns>
    public static int[] MergeLeft(int[] line)
    {
        //防止传入空数组
        if (line == null)
            return null;

        //第一步：移除所有0
        List<int> numbers = new List<int>();

        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] != 0)
            {
                numbers.Add(line[i]);
            }
        }

        //第二步：合并相邻且相同的数字
        for (int i = 0; i < numbers.Count - 1; i++)
        {
            if (numbers[i] == numbers[i + 1])
            {
                numbers[i] *= 2;
                numbers.RemoveAt(i + 1);
            }
        }

        //第三步：创建与原数组相同长度的新数组
        int[] result = new int[line.Length];

        //将合并后的数字放到左边
        for (int i = 0; i < numbers.Count; i++)
        {
            result[i] = numbers[i];
        }

        return result;
    }
}