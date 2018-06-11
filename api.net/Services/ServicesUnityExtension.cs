using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Extension;
using Unity.Lifetime;

namespace MuzerAPI
{
    public class ServicesUnityExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.AddExtension(new RepositoryUnityExtension());

            Container.RegisterType<AlbumService.AlbumService, AlbumService.AlbumService>(new HierarchicalLifetimeManager());
            Container.RegisterType<ArtistService.ArtistService, ArtistService.ArtistService>(new HierarchicalLifetimeManager());
        }
    }
}
