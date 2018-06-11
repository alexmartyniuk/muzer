using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuzerAPI.Repositories;
using Unity;
using Unity.Extension;
using Unity.Lifetime;

namespace MuzerAPI
{
    public class RepositoryUnityExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType<DatabaseContext, DatabaseContext>(new HierarchicalLifetimeManager());

            Container.RegisterType<AlbumRepository, AlbumRepository>(new HierarchicalLifetimeManager());
            Container.RegisterType<ArtistRepository, ArtistRepository>(new HierarchicalLifetimeManager());
            Container.RegisterType<TrackRepository, TrackRepository>(new HierarchicalLifetimeManager());
        }
    }
}
