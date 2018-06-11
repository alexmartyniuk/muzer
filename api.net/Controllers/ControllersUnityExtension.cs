using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity.Extension;

namespace MuzerAPI
{
    public class ControllersUnityExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.AddExtension(new ServicesUnityExtension());
        }
    }
}