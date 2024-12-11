using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDWeave.Godot.Variants;
using GDWeave.Godot;
using GDWeave.Modding;
using Serilog.Core;


public class BlockerBotBypass : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Menus/Loading Menu/loading_menu.gdc";

    // returns a list of tokens for the new script, with the input being the original script's tokens
    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        // wait for any newline token after any extends token
        MultiTokenWaiter waiter = new MultiTokenWaiter(new Func<Token, bool>[2]
            {
            (Token t) => t.Type == TokenType.Constant,
            delegate(Token t) {
                ConstantToken val = (ConstantToken)(object)((t is ConstantToken) ? t : null);
                if (val == null)
                {
                    return false;
                }
                if (val.Value is IntVariant v)
                {
                    return v.Equals(new IntVariant(6));
                } else
                {
                    return false;
                }
            }
            }, false, false);

        // loop through all tokens in the script
        foreach (var token in tokens)
        {
            
            if (waiter.Check(token))
            {
                // found our match, return the original newline
                yield return token;

                // then add our own code
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("packets_recieved");
                yield return new Token(TokenType.OpGreaterEqual);

                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.OpOr);
                yield return new IdentifierToken("packets_recieved");
                yield return new Token(TokenType.OpGreaterEqual);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("packets_recieved");
                yield return new IdentifierToken("Network.LOBBY_MEMBERS.size");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpSub);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);

                yield return new Token(TokenType.Newline);

            }
            else
            {
                // return the original token
                yield return token;
            }
        }
    }
}
