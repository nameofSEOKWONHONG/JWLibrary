using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace JWLibrary.Core {
    public static class JLamda {
        static string jToString(Expression expr) {
            switch (expr.NodeType) {
                case ExpressionType.Lambda:
                    //x => (Something), return only (Something), the Body
                    return jToString(((LambdaExpression)expr).Body);
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    //type casts are not important
                    return jToString(((UnaryExpression)expr).Operand);
                case ExpressionType.Call:
                    //method call can be an Indexer (get_Item),
                    var callExpr = (MethodCallExpression)expr;
                    if (callExpr.Method.Name == "get_Item") {
                        //indexer call
                        return jToString(callExpr.Object) + "[" + string.Join(",", callExpr.Arguments.Select(jToString)) + "]";
                    } else {
                        //method call
                        var arguments = callExpr.Arguments.Select(jToString).ToArray();
                        string target;
                        if (callExpr.Method.IsDefined(typeof(ExtensionAttribute), false)) {
                            //extension method
                            target = string.Join(".", arguments[0], callExpr.Method.Name);
                            arguments = arguments.Skip(1).ToArray();
                        } else if (callExpr.Object == null) {
                            //static method
                            target = callExpr.Method.Name;
                        } else {
                            //instance method
                            target = string.Join(".", jToString(callExpr.Object), callExpr.Method.Name);
                        }
                        return target + "(" + string.Join(",", arguments) + ")";
                    }
                case ExpressionType.MemberAccess:
                    //property or field access
                    var memberExpr = (MemberExpression)expr;
                    if (memberExpr.Expression.Type.Name.Contains("<>")) //closure type, don't show it.
                    {
                        return memberExpr.Member.Name;
                    } else {
                        return string.Join(".", jToString(memberExpr.Expression), memberExpr.Member.Name);
                    }
            }
            //by default, show the standard implementation
            return expr.ToString();
        }
    }
}
