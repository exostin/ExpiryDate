using TMPro;
using UnityEngine;

namespace Other
{
    public class GetVersion : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI versionText;

        private void Start()
        {
            versionText.text = "v" + Application.version;
        }
    }
}