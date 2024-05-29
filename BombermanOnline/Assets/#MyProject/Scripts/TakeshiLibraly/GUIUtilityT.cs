using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakeshiLibrary
{
    public class GUIUtilityT : MonoBehaviour
    {

        /// <summary>
        /// GUI�X�^�C����ݒ肵�܂�
        /// </summary>
        public static void SetGUIStyle(GUIStyle gUIStyle, Color trueColor, Color falseColor, Color pushColor)
        {
            var trueTexture = MakeTexture(Figures.TwoPowerdByFive, Figures.TwoPowerdByFive, trueColor);
            var falseTexture = MakeTexture(Figures.TwoPowerdByFive, Figures.TwoPowerdByFive, falseColor);
            var pushTexture = MakeTexture(Figures.TwoPowerdByFive, Figures.TwoPowerdByFive, pushColor);
            SetGUIStyle(gUIStyle, trueTexture, falseTexture, pushTexture);
        }
        public static void SetGUIStyle(GUIStyle gUIStyle, Texture2D trueTexture, Texture2D falseTextrue, Texture2D pushTexture)
        {
            // �e�N�X�`����ݒ�
            if (trueTexture == null || trueTexture == null || pushTexture == null)
            {
                Debug.LogError("�e�N�X�`���t�@�C����������܂���I");
            }
            else
            {
                // true
                gUIStyle.onNormal.background = trueTexture;
                // false
                gUIStyle.normal.background = falseTextrue;

                // �ς��u��
                gUIStyle.active.background = pushTexture;
                gUIStyle.onActive.background = pushTexture;
            }
        }


        /// <summary>
        /// �e�N�X�`�����쐬���܂�
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Texture2D MakeTexture(int width, int height, Color color)
        {
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            Texture2D texture = new Texture2D(width, height);
            texture.SetPixels(pixels);
            texture.Apply();

            return texture;
        }
    }
}