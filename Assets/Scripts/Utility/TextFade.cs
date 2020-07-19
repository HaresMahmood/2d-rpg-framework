using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace CharTween.Examples
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextFade : MonoBehaviour
    {
        #region Constants

        // Lookup table for hex characters.
        private static readonly char[] hex = new char[] {
        '0', '1', '2', '3',
        '4', '5', '6', '7',
        '8', '9', 'A', 'B',
        'C', 'D', 'E', 'F'
    };

        #endregion

        #region Variables

        [SerializeField] private float fadeDuration = 0.2f; // Number of seconds each character should take to fade up
        [SerializeField] private float travelSpeed = 20f; // Speed the reveal travels along the text, in characters per second

        private TextMeshProUGUI text;

        private Task fade;

        #endregion

        #region Miscellaneous Methods

        public void FadeTo(string text)
        {
            // Abort a fade in progress, if any.
            StopFade();

            // Start fading, and keep track of the coroutine so we can interrupt if needed.
            fade = new Task(FadeText(text));
        }

        public void StopFade()
        {
            if (fade != null)
            {
                fade.Stop();
            }
        }

        // Currently this expects a string of plain text,
        // and will not correctly handle rich text tags etc.
        private IEnumerator FadeText(string text)
        {
            /*
            this.text.text = text;
            text = StripTags(text);

            int length = text.Length;

            // Build a character buffer of our desired text,
            // with a rich text "color" tag around every character.
            var builder = new System.Text.StringBuilder(length * 26);
            Color32 color = this.text.color;
            for (int i = 0; i < length; i++)
            {
                builder.Append("<color=#");
                builder.Append(hex[color.r >> 4]);
                builder.Append(hex[color.r & 0xF]);
                builder.Append(hex[color.g >> 4]);
                builder.Append(hex[color.g & 0xF]);
                builder.Append(hex[color.b >> 4]);
                builder.Append(hex[color.b & 0xF]);
                builder.Append("00>");
                builder.Append(text[i]);
                builder.Append("</color>");
            }

            // Each frame, update the alpha values along the fading frontier.
            float fadingProgress = 0f;
            int opaqueChars = -1;
            while (opaqueChars < length - 1)
            {
                yield return null;

                fadingProgress += Time.deltaTime;

                float leadingEdge = fadingProgress * travelSpeed;

                int lastChar = Mathf.Min(length - 1, Mathf.FloorToInt(leadingEdge));

                int newOpaque = opaqueChars;

                for (int i = lastChar; i > opaqueChars; i--)
                {
                    byte fade = (byte)(255f * Mathf.Clamp01((leadingEdge - i) / (travelSpeed * fadeDuration)));
                    builder[i * 26 + 14] = hex[fade >> 4];
                    builder[i * 26 + 15] = hex[fade & 0xF];

                    if (fade == 255)
                        newOpaque = Mathf.Max(newOpaque, i);
                }

                opaqueChars = newOpaque;

                // This allocates a new string.
                this.text.text = builder.ToString();
                }

                        // Once all the characters are opaque, 
                // ditch the unnecessary markup and end the routine.
                this.text.text = text;

            // Mark the fade transition as finished.
            // This can also fire an event/message if you want to signal UI.
            fade = null;

                */

            this.text.text = text;

            var tweener = this.text.GetCharTweener();

            /*
            for (var i = 0; i < tweener.CharacterCount; i++)
            {
                // Oscillate character color between yellow and white
                var colorTween = tweener.DOFade(i, 1f, fadeDuration);

                // Offset animations based on character index in string
                var timeOffset = Mathf.Lerp(0, 1, i / (float)(tweener.CharacterCount - 1));
                colorTween.fullPosition = timeOffset;
            }
            */

            var sequence = DOTween.Sequence();
            int start = 0; int end = tweener.CharacterCount;

            for (var i = 0; i <= tweener.CharacterCount; ++i)
            {
                var timeOffset = Mathf.Lerp(0, 1, (i - start) / (float)(end - start + 1));
                var charSequence = DOTween.Sequence();
                charSequence.Append(tweener.DOFade(i, 0, 0.5f).From())
                    .OnComplete(() => {
                    Debug.Log("Done");
                });;
                sequence.Insert(timeOffset, charSequence);
            }

            //sequence.SetLoops(-1, LoopType.Yoyo);
            
            yield break;
        }

        private string StripTags(string text)
        {
            string pattern = @"<(.|\n)*?>";

            return Regex.Replace(text, pattern, string.Empty);
        }

        #endregion

        #region Unity Methods

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        #endregion
    }
}