using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Rubberduck.Inspections.Abstract;
using Rubberduck.Inspections.Results;
using Rubberduck.Parsing;
using Rubberduck.Parsing.Grammar;
using Rubberduck.Parsing.Inspections.Abstract;
using Rubberduck.Parsing.Inspections.Resources;
using Rubberduck.Parsing.VBA;
using Rubberduck.VBEditor;

namespace Rubberduck.Inspections.Concrete
{
    public sealed class MultilineParameterInspection : InspectionBase, IParseTreeInspection
    {
        public MultilineParameterInspection(RubberduckParserState state)
            : base(state, CodeInspectionSeverity.Suggestion) { }

        public override CodeInspectionType InspectionType => CodeInspectionType.MaintainabilityAndReadabilityIssues;

        public IInspectionListener Listener { get; } =
            new ParameterListener();

        public override IEnumerable<IInspectionResult> GetInspectionResults()
        {
            if (Listener?.Contexts == null)
            {
                return Enumerable.Empty<IInspectionResult>();
            }
            return Listener.Contexts
                .Where(result => !IsIgnoringInspectionResultFor(result.ModuleName.Component, result.Context.Start.Line))
                .Select(p => new MultilineParameterInspectionResult(this, p));
        }

        public class ParameterListener : VBAParserBaseListener, IInspectionListener
        {
            private readonly List<QualifiedContext<ParserRuleContext>> _contexts = new List<QualifiedContext<ParserRuleContext>>();
            public IReadOnlyList<QualifiedContext<ParserRuleContext>> Contexts => _contexts;

            public QualifiedModuleName CurrentModuleName { get; set; }

            public void ClearContexts()
            {
                _contexts.Clear();
            }
            
            public override void ExitArg([NotNull] VBAParser.ArgContext context)
            {
                if (context.Start.Line != context.Stop.Line)
                {
                    _contexts.Add(new QualifiedContext<ParserRuleContext>(CurrentModuleName, context));
                }
            }
        }
    }
}
