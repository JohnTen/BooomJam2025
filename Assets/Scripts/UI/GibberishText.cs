using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public class GibberishText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float updateDelay = 0.1f; // 更新延迟时间
    [SerializeField] [Range(0f, 1f)] private float gibberishLevel = 0.5f; // 乱码程度，0-1之间
    [SerializeField] private bool levelFromGameManager = true;
    [SerializeField] private bool useRandomSeed = true;

    static float[] gibberishStaticLevels = {
        0.2f,
        0.4f,
        0.6f,
        0.8f,
        1f,
    };

    static float[] gibberishStaticRefs = {
        0f,
        0.25f,
        0.5f,
        0.75f,
        1f,
    };

    private string originalText = "";
    private string lastProcessedText = "";
    private Coroutine updateCoroutine;
    private byte[] hashKey;
    private bool isProcessing = false;

    private float lastLevel = 0;

    private int seed = 0;

    private void Awake()
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        seed = UnityEngine.Random.Range(0, int.MaxValue);
    }

    private void OnEnable()
    {
        if (text != null)
        {
            lastProcessedText = text.text;
            originalText = RemoveHtmlTags(text.text);
            UpdateGibberishText();
        }
    }

    private void Update()
    {
        if (levelFromGameManager)
        {
            gibberishLevel = 1 - GameManager.Instance.corePercent[CoreSlotType.MemoryCore];
        }

        if (text != null && !isProcessing)
        {
            if (text.text != lastProcessedText)
            {
                SetText(text.text);
            }
            if (gibberishLevel != lastLevel)
            {
                SetGibberishLevel(gibberishLevel);
            }
        }
    }

    private string RemoveHtmlTags(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        // 移除所有HTML标签，包括<sprite>标签
        return Regex.Replace(input, "<[^>]*>", "");
    }

    public void SetText(string newText)
    {
        if (string.IsNullOrEmpty(newText)) return;
        
        originalText = RemoveHtmlTags(newText);
        if (updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
        }
        updateCoroutine = StartCoroutine(UpdateTextWithDelay());
    }

    public void SetGibberishLevel(float level)
    {
        gibberishLevel = Mathf.Clamp01(level);
        if (!string.IsNullOrEmpty(originalText))
        {
            if (updateCoroutine != null)
            {
                StopCoroutine(updateCoroutine);
            }
            updateCoroutine = StartCoroutine(UpdateTextWithDelay());
        }
        lastLevel = gibberishLevel;
    }

    private IEnumerator UpdateTextWithDelay()
    {
        isProcessing = true;
        yield return new WaitForSeconds(updateDelay);
        text.text = ConvertToGibberish(originalText);
        lastProcessedText = text.text;
        isProcessing = false;
    }

    private void UpdateGibberishText()
    {
        if (string.IsNullOrEmpty(originalText)) return;
        text.text = ConvertToGibberish(originalText);
        lastProcessedText = text.text;
    }

    private string ConvertToGibberish(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        if (gibberishLevel <= 0) return input;

        var level = 0f;
        if (gibberishStaticLevels.Length > 0)
        {
            var index = 0;
            foreach (var staticLevel in gibberishStaticLevels)
            {
                if (staticLevel >= gibberishLevel)
                {
                    break;
                }
                index++;
            }
            level = gibberishStaticRefs[index];
        }
        else
        {
            level = gibberishLevel;
        }

        // 生成基于文本和level的哈希密钥
        string hashInput = input + level.ToString("F6");
        if (useRandomSeed)
        {
            hashInput += seed.ToString();
        }
        using (var sha256 = SHA256.Create())
        {
            hashKey = sha256.ComputeHash(Encoding.UTF8.GetBytes(hashInput));
        }

        StringBuilder result = new StringBuilder();
        int totalChars = 0;
        int convertedChars = 0;

        for (int i = 0; i < input.Length; i++)
        {
            char currentChar = input[i];
            
            // 跳过空格和标点符号
            if (char.IsWhiteSpace(currentChar) || char.IsPunctuation(currentChar))
            {
                result.Append(currentChar);
                continue;
            }

            totalChars++;
            // 使用位置信息和哈希密钥生成一个0-1之间的值
            float positionValue = GetPositionValue(i, totalChars);
            
            // 根据当前已转换的字符比例动态调整转换概率
            float currentRatio = (float)convertedChars / totalChars;
            float adjustedLevel = level + (level - currentRatio) * 0.5f;
            
            if (positionValue <= adjustedLevel)
            {
                int spriteIndex = GetSpriteIndex(i, currentChar);
                result.Append($"<sprite=\"CuAb\" index={spriteIndex}>");
                convertedChars++;
            }
            else
            {
                result.Append(currentChar);
            }
        }

        return result.ToString();
    }

    private float GetPositionValue(int position, int totalChars)
    {
        // 使用HMAC-SHA256生成基于位置的哈希值
        using (var hmac = new HMACSHA256(hashKey))
        {
            byte[] positionBytes = BitConverter.GetBytes(position);
            byte[] totalBytes = BitConverter.GetBytes(totalChars);
            byte[] combinedBytes = new byte[positionBytes.Length + totalBytes.Length];
            Buffer.BlockCopy(positionBytes, 0, combinedBytes, 0, positionBytes.Length);
            Buffer.BlockCopy(totalBytes, 0, combinedBytes, positionBytes.Length, totalBytes.Length);
            
            byte[] hash = hmac.ComputeHash(combinedBytes);
            // 使用哈希的前4个字节生成0-1之间的值
            return (float)BitConverter.ToUInt32(hash, 0) / uint.MaxValue;
        }
    }

    private int GetSpriteIndex(int position, char currentChar)
    {
        // 使用HMAC-SHA256生成基于字符和位置的精灵索引
        using (var hmac = new HMACSHA256(hashKey))
        {
            byte[] positionBytes = BitConverter.GetBytes(position);
            byte[] charBytes = BitConverter.GetBytes(currentChar);
            byte[] combinedBytes = new byte[positionBytes.Length + charBytes.Length];
            Buffer.BlockCopy(positionBytes, 0, combinedBytes, 0, positionBytes.Length);
            Buffer.BlockCopy(charBytes, 0, combinedBytes, positionBytes.Length, charBytes.Length);
            
            byte[] hash = hmac.ComputeHash(combinedBytes);
            // 使用哈希的前4个字节生成0-29之间的索引
            return (int)(BitConverter.ToUInt32(hash, 0) % 30);
        }
    }
}
