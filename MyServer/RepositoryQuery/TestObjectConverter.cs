//using System.Linq.Expressions;

//namespace MyServer.RepositoryQuery
//{
//    public class TestObjectConverter: ConverterBase<toViewObject, baseObject>
//    {
//        public override IEnumerable<Expression<Func<baseObject, object>>> Projections
//        {
//            get
//            {
//                return new Expression<Func<baseObject, object>>[]
//                {
//                    x => x.field1,//0
//                    x => x.field2,//1
//                    x => x.field3,//2
//                    x => x.field4 //3
//                };
//            }
//        }
//    }

//    protected override toViewObject Convert(object[] tuple)
//    {
//        var field1 = (type)tuple[0];
//        //...
//        var field4 = (type)tuple[3];
//        return new toViewObject(field1, field4);
//    }
//}
