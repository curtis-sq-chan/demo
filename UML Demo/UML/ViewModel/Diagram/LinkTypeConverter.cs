using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.ViewModel.Diagram
{
    public class LinkTypeConverter
    {
        static public Link.LinkType Convert( Domain.Diagram.Relationship.RelationshipType type )
        {
            Link.LinkType returnType;

            switch (type)
            {
                case Domain.Diagram.Relationship.RelationshipType.Aggregate:
                    returnType = Link.LinkType.Aggregate;
                    break;
                case Domain.Diagram.Relationship.RelationshipType.Composite:
                    returnType = Link.LinkType.Composite;
                    break;
                case Domain.Diagram.Relationship.RelationshipType.Dependency:
                    returnType = Link.LinkType.Dependency;
                    break;
                case Domain.Diagram.Relationship.RelationshipType.Inheritance:
                    returnType = Link.LinkType.Inheritance;
                    break;
                default:
                    throw new Exception("Invalid argument type.");
            }

            return returnType;
        }

        static public Domain.Diagram.Relationship.RelationshipType Convert(Link.LinkType type)
        {
            Domain.Diagram.Relationship.RelationshipType returnType;

            switch( type )
            {
                case Link.LinkType.Aggregate:
                    returnType = Domain.Diagram.Relationship.RelationshipType.Aggregate;
                    break;
                case Link.LinkType.Composite:
                    returnType = Domain.Diagram.Relationship.RelationshipType.Composite;
                    break;
                case Link.LinkType.Dependency:
                    returnType = Domain.Diagram.Relationship.RelationshipType.Dependency;
                    break;
                case Link.LinkType.Inheritance:
                    returnType = Domain.Diagram.Relationship.RelationshipType.Inheritance;
                    break;
                default:
                    throw new Exception("Invalid argument type.");
            }

            return returnType;
        }
    }
}
