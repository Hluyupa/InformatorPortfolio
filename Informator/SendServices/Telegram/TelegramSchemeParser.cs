using Html2Markdown.Replacement;
using Html2Markdown.Scheme;
using System.Text.RegularExpressions;

namespace Informator.SendServices.Telegram;

internal class TelegramSchemeParser : AbstractScheme {
    public TelegramSchemeParser() {
        AddReplacementGroup(_replacers, new TextFormattingCustom());
		AddReplacementGroup(_replacers, new HeadingReplacementGroup());
		AddReplacementGroup(_replacers, new IllegalHtmlReplacementGroup());
		AddReplacementGroup(_replacers, new LayoutReplacementGroup());
		AddReplacementGroup(_replacers, new EntitiesReplacementGroup());
	}
}

internal class TextFormattingCustom : IReplacementGroup {
    private readonly IList<IReplacer> _replacers = new List<IReplacer>() {
		new PatternReplacerCustom
		{
			Pattern = "</?p[^>]*>",
			Replacement = ""
		}
		/*
		new CustomReplacer {
			CustomAction = html => {
				var regex = new Regex(@"(\s+|@|&|'|\(|\)|<|>|#|!)");
				foreach (Match match in regex.Matches(html)) {
					match.Result()
                }

				var replacement = Regex.Replace(html,@"(\s+|@|&|'|\(|\)|<|>|#)", String.Empty);
				return replacement;
            }
        }*/
	};
    
    public IList<IReplacer> Replacers() {
        return _replacers;
    }

    internal class PatternReplacerCustom : IReplacer {
        public string Pattern { get; set; }
        public string Replacement { get; set; }
        public string Replace(string html) {
            var regex = new Regex(Pattern);

            return regex.Replace(html, Replacement);
        }
    }

	internal class CustomReplacer : IReplacer {
		public string Replace(string html) {
			return CustomAction.Invoke(html);
		}

		public Func<string, string> CustomAction { get; set; }
	}
}
