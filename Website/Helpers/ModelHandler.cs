using AutoMapper;
using System;
using Website.Models;
using Website.Models.Shared.Factories;

namespace Website.Helpers
{
    public class ModelHandler
    {
        private MenuFactory _menuFactory;
        private IMapper _mapper;

        public ModelHandler(MenuFactory menuFactory, IMapper mapper)
        {
            _menuFactory = menuFactory;
            _mapper = mapper;
        }
        
        public IBaseViewModel Create()
        {
            return Create<BaseViewModel>();
        }

        internal IBaseViewModel Create<T>()
        {
            if (typeof(IBaseViewModel).IsAssignableFrom(typeof(T)))
            {
                IBaseViewModel model = (IBaseViewModel)Activator.CreateInstance(typeof(T));


                // TODO - Grab T's Factory type and invoke that factory
                model.Menu = _menuFactory.Create();

                return model;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
