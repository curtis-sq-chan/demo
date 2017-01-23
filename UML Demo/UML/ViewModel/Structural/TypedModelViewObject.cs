using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UML.ViewModel.Structural
{
    public abstract class TypedModelViewObject : ModelViewObject
    {
        private Crosscutting.Session m_session;
        private List<string> m_typeList = new List<string>();

        // COMMANDS
        private ICommand m_refreshTypeManifest;

        public TypedModelViewObject( Crosscutting.Session session )
        {
            m_session = session;
            m_refreshTypeManifest = new RelayCommand(OnRefreshTypeManifest);
        }

        private void OnRefreshTypeManifest(object obj)
        {
            List<string> results = new List<string>();
            results.Add("float");
            results.Add("double");
            results.Add("int");
            results.Add("char");
            results.Add("string");
            results.Add("long");
            results.Add("void");
            results.Add("long");
            results.Add("bool");
            results.Add("short");

            Repository.Local.ItemRepository<Domain.Structural.Class> classRepository = Repository.Local.RepositoryFactory.NewClassRepository(m_session);
            IEnumerable<Repository.ItemMetadata> classes = classRepository.GetAll();

            foreach (Repository.ItemMetadata currentClass in classes)
            {
                results.Add(currentClass.Name);
            }
            TypeManifest = results;
        }

        public List<string> TypeManifest
        {
            get { return m_typeList; }
            private set
            {
                m_typeList = value;
                OnPropertyChanged("TypeManifest");
            }
        }

        public ICommand GetTypeManifest
        {
            get { return m_refreshTypeManifest; }
        }
    }
}
