using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using InstallationValidator.Core.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Core.Domain.Constants;

namespace InstallationValidator.Core.Domain
{
   public class ComparisonSettings
   {
      private HashSet<string> _exclusionHash = new HashSet<string>();
      private IReadOnlyList<Regex> _regexExclusions = new Regex[] { };
      private bool _hasExclusion;
      private static readonly string ALL_BUT_PATH_DELIMITER = $"[^{ObjectPath.PATH_DELIMITER}]*";
      private static readonly string PATH_DELIMITER = $"\\{ObjectPath.PATH_DELIMITER}";
      private static readonly string OPTIONAL_PATH_DELIMITER = $"(\\{PATH_DELIMITER})?";

      /// <summary>
      ///    First folder used for comparison
      /// </summary>
      public string FolderPath1 { get; set; }

      /// <summary>
      ///    Second folder used for comparison
      /// </summary>
      public string FolderPath2 { get; set; }

      public string FolderPathCaption1 { get; set; } = Captions.DefaultCaptionFolder1;

      public string FolderPathCaption2 { get; set; } = Captions.DefaultCaptionFolder2;

      public bool GenerateResultsForValidSimulation { get; set; } = false;

      public IReadOnlyList<string> PredefinedOutputPaths { get; set; } = new List<string>();

      /// <summary>
      ///    Number of curves to add to the report when file being compared are not the same
      /// </summary>
      public byte? NumberOfCurves { get; set; }

      /// <summary>
      ///    Should curve added to the model structure be ignored. Default is <code>false</code>
      /// </summary>
      public bool IgnoreAddedCurves { get; set; }

      /// <summary>
      ///    Should curve removed from the model structure be ignored. Default is <code>false</code>
      /// </summary>
      public bool IgnoreRemovedCurves { get; set; }

      /// <summary>
      ///    List of path to exclude from the comparison. This can contain wild cards
      /// </summary>
      public IReadOnlyCollection<string> Exclusions
      {
         get => _exclusionHash;
         set
         {
            _exclusionHash = new HashSet<string>(value ?? Array.Empty<string>());
            _hasExclusion = _exclusionHash.Any();
            _regexExclusions = _exclusionHash.Where(x => x.Contains(WILD_CARD))
               .Select(x => new Regex(createSearchPattern(x.ToPathArray()), RegexOptions.IgnoreCase))
               .ToList();
         }
      }

      public bool CanCompare(string path)
      {
         if (!_hasExclusion)
            return true;

         if (_exclusionHash.Contains(path))
            return false;

         return !_regexExclusions.Any(x => x.IsMatch(path));
      }

      private string createSearchPattern(string[] path)
      {
         var pattern = new List<string>();
         foreach (var entry in path)
         {
            if (string.Equals(entry, WILD_CARD))
            {
               // At least one occurrence of a path entry => anything except ObjectPath.PATH_DELIMITER, repeated once
               pattern.Add($"{ALL_BUT_PATH_DELIMITER}?");
               pattern.Add(PATH_DELIMITER);
            }
            else if (string.Equals(entry, WILD_CARD_RECURSIVE))
            {
               pattern.Add(".*"); //Match anything
               pattern.Add(OPTIONAL_PATH_DELIMITER);
            }
            else
            {
               pattern.Add(entry
                  .Replace(WILD_CARD, ALL_BUT_PATH_DELIMITER)
                  .Replace("(", "\\(")
                  .Replace(")", "\\)"));
               pattern.Add(PATH_DELIMITER);
            }
         }

         pattern.RemoveAt(pattern.Count - 1);
         var searchPattern = pattern.ToString("");
         return $"^{searchPattern}$";
      }
   }
}