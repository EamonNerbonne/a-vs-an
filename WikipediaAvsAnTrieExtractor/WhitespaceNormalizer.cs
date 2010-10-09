using System.Text;

namespace WikiParser
{
    public static class WhitespaceNormalizer
    {
        /// <summary>
        /// This normalizes a string such that consecutive whitespace and tabs are replaced by a single space,
        /// and such that any leading or trailing whitespace on any line gets trimmed.  Sequences of empty lines 
        /// are replaced by a single empty line.  After the last normal character, at most one line-break is permitted.
        /// Implementation limitation: before the first character on the first line, a single whitespace will not be removed.
        /// 
        /// This implementation is somewhat odd, but the regex implementation is surprisingly slow due to backtracking issues
        /// which arrise from the matching of consecutive empty lines (which might contain white space).  The purpose of this 
        /// implementation is to essentially remove superfluous spaces being those that lead or trail any line and to remove
        /// superflous empty lines, such that a single empty line is still permitted (being a wikipedia paragraph break).
        /// </summary>
        public static string Normalize(string text) {

            StringBuilder s = new StringBuilder(text.Length);
            char c;

            for (int i = 0; i < text.Length; i++) {
                c = text[i];

                if (c == ' ' || c == '\t') {
                    while (true) {
                        if (++i >= text.Length) {
                            break;
                        }
                        c = text[i];


                        if (c == '\n') {
                            while (true) {
                                if (++i >= text.Length) {
                                    s.Append('\n');
                                    break;
                                }
                                c = text[i];

                                if (c == '\n') {
                                    while (true) {
                                        if (++i >= text.Length) {
                                            s.Append('\n');
                                            break;
                                        }
                                        c = text[i];

                                        if (!(c == ' ' || c == '\t' || c == '\n')) {
                                            s.Append('\n');
                                            s.Append('\n');
                                            s.Append(c);
                                            break;
                                        }
                                    }
                                    break;
                                } else if (!(c == ' ' || c == '\t')) {
                                    s.Append('\n');
                                    s.Append(c);
                                    break;
                                }
                            }
                            break;
                        } else if (!(c == ' ' || c == '\t')) {
                            s.Append(' ');
                            s.Append(c);
                            break;
                        }
                    }
                } else if (c == '\n') {
                    while (true) {
                        if (++i >= text.Length) {
                            s.Append('\n');
                            break;
                        }
                        c = text[i];

                        if (c == '\n') {
                            while (true) {
                                if (++i >= text.Length) {
                                    s.Append('\n');
                                    break;
                                }
                                c = text[i];

                                if (!(c == ' ' || c == '\t' || c == '\n')) {
                                    s.Append('\n');
                                    s.Append('\n');
                                    s.Append(c);
                                    break;
                                }
                            }
                            break;
                        } else if (!(c == ' ' || c == '\t')) {
                            s.Append('\n');
                            s.Append(c);
                            break;
                        }
                    }
                } else {
                    s.Append(c);
                }
            }
            return s.ToString();
        }

    }
}
