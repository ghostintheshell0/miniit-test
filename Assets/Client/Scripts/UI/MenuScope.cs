using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace miniIT.Arcanoid
{
    public class MenuScope : LifetimeScope
    {
        [SerializeField]
        private MenuController menuController = default;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(menuController);
            base.Configure(builder);
        }
    }
}