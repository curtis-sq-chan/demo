using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.ViewModel.Structural
{
    class ClassFactory
    {
        public Class CreateClass( Crosscutting.Session session )
        {
            Domain.Structural.Class newClass = new Domain.Structural.Class();
            newClass.Id = Guid.NewGuid();

            Repository.Local.ItemRepository<Domain.Structural.Class> classRepository = Repository.Local.RepositoryFactory.NewClassRepository(session);
            classRepository.Add(newClass);

            Structural.Class newClassVM = new Structural.Class(session, newClass);

            return newClassVM;
        }

        public Class CreateClass(Crosscutting.Session session, Guid classId)
        {
            Repository.Local.ItemRepository<Domain.Structural.Class> classRepository = Repository.Local.RepositoryFactory.NewClassRepository(session);
            Domain.Structural.Class newClass = classRepository.GetById(classId);
            Structural.Class newClassVM = new Structural.Class( session, newClass);

            return newClassVM;
        }
    }
}
