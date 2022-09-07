using GeoChatter.Core.Common.Extensions;
using GeoChatter.Helpers;
using GeoChatter.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoChatter.Core.Model
{
    /// <summary>
    /// Score formulator expression model, containing the built-in variables
    /// </summary>
    public sealed class ScoreFormulatorExpression
    {
        /// <summary>
        /// Create an expression using <paramref name="guess"/> data
        /// </summary>
        /// <param name="guess"></param>
        /// <param name="otherRounds"></param>
        /// <returns></returns>
        public static ScoreFormulatorExpression Create(Guess guess, ICollection<Round> otherRounds = null)
        {
            GCUtils.ThrowIfNull(guess, nameof(guess));

            Round round = guess.Round;
            double scale = GameHelper.CalculateScale(round.Game.Bounds);
            return new()
            {
                GuesserNumber = new(round.Guesses.Count),
                DistanceMeters = new(guess.Distance * 1000),
                CorrectLatitude = new(round.CorrectLocation.Latitude),
                CorrectLongitude = new(round.CorrectLocation.Longitude),
                GuessLatitude = new(guess.GuessLocation.Latitude),
                GuessLongitude = new(guess.GuessLocation.Latitude),
                DefaultScore = new(guess.Score),
                GuessesMade = new(otherRounds == null ? 0 : otherRounds.Count),
                CorrectCountry = new(round.Country.Code, round.Country.Name),
                CorrectRegion = new(round.ExactCountry.Code, round.ExactCountry.Name),
                GuessCountry = new(guess.Country.Code, guess.Country.Name),
                GuessRegion = new(guess.CountryExact.Code, guess.CountryExact.Name),
                RoundNumber = new(round.RoundNumber),
                Streak = new(guess.Player.CountryStreak),
                Time = new(guess.Time),
                IsRandomGuess = new(guess.WasRandom),
                MapScale = new(scale)
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public ScoreFormulatorExpression()
        {

        }

        /// <summary>
        /// Create expression basis from given value
        /// </summary>
        /// <param name="clat"></param>
        /// <param name="clng"></param>
        /// <param name="glat"></param>
        /// <param name="glng"></param>
        /// <param name="def"></param>
        /// <param name="dist"></param>
        /// <param name="b"></param>
        /// <param name="cc"></param>
        /// <param name="gc"></param>
        /// <param name="cr"></param>
        /// <param name="gr"></param>
        public ScoreFormulatorExpression(string clat, string clng, string glat, string glng, string def, string dist, ScoreFormulatorExpression b,
            Country cc, Country gc, Country cr, Country gr)
        {
            if (b != null)
            {
                GuesserNumber = new(b.GuesserNumber);
                DistanceMeters = new(b.DistanceMeters);
                CorrectLatitude = new(clat);
                CorrectLongitude = new(clng);
                GuessLatitude = new(glat);
                GuessLongitude = new(glng);
                DefaultScore = new(def);
                DistanceMeters = new(dist);
                GuessesMade = new(b.GuessesMade);
                CorrectCountry = cc;
                CorrectRegion = cr;
                GuessCountry = gc;
                GuessRegion = gr;
                RoundNumber = new(b.RoundNumber);
                Streak = new(b.Streak);
                Time = new(b.Time);
                IsRandomGuess = new(b.IsRandomGuess);
                MapScale = new(b.MapScale);
            }
        }

        private static Random random { get; } = new();

        /// <summary>
        /// Guess latitude
        /// </summary>
        public ScoreFormulatorVariable GuessLatitude { get; private set; } = ScoreFormulatorVariable.False;
        /// <summary>
        /// Guess longitude
        /// </summary>
        public ScoreFormulatorVariable GuessLongitude { get; private set; } = ScoreFormulatorVariable.False;
        /// <summary>
        /// Correct latitude
        /// </summary>
        public ScoreFormulatorVariable CorrectLatitude { get; private set; } = ScoreFormulatorVariable.False;
        /// <summary>
        /// Correct longitude
        /// </summary>
        public ScoreFormulatorVariable CorrectLongitude { get; private set; } = ScoreFormulatorVariable.False;

        /// <summary>
        /// Correct country
        /// </summary>
        private Country CorrectCountry { get; set; }
        /// <summary>
        /// Guessed country
        /// </summary>
        private Country GuessCountry { get; set; }
        /// <summary>
        /// Correct region
        /// </summary>
        private Country CorrectRegion { get; set; }
        /// <summary>
        /// Guessed region
        /// </summary>
        private Country GuessRegion { get; set; }
        /// <summary>
        /// Wheter <see cref="CorrectCountry"/> is same as <see cref="GuessCountry"/>
        /// </summary>
        public ScoreFormulatorVariable IsCorrectCountry => CorrectCountry != null && GuessCountry != null && CorrectCountry.IsSame(GuessCountry) ? ScoreFormulatorVariable.True : ScoreFormulatorVariable.False;
        /// <summary>
        /// Wheter <see cref="CorrectRegion"/> is same as <see cref="GuessRegion"/>
        /// </summary>
        public ScoreFormulatorVariable IsCorrectRegion => CorrectRegion != null && GuessRegion != null && CorrectRegion.IsSame(GuessRegion) ? ScoreFormulatorVariable.True : ScoreFormulatorVariable.False;
        /// <summary>
        /// Wheter guess made is random
        /// </summary>
        public ScoreFormulatorVariable IsRandomGuess { get; private set; } = ScoreFormulatorVariable.False;
        /// <summary>
        /// A Diverse World map scale
        /// </summary>
        public static ScoreFormulatorVariable MapScaleADW { get; } = new(GameHelper.CalculateScale(new Bounds() { Min = new(-85.00001648414728, -177.3805254279055), Max = new(81.6812215033565, 178.3898080125991) }));
        /// <summary>
        /// Map scale
        /// </summary>
        public ScoreFormulatorVariable MapScale { get; private set; } = new(MapScaleADW);
        /// <summary>
        /// Perfect score value
        /// </summary>
        public static ScoreFormulatorVariable PerfectScoreStatic { get; set; } = new(5000D);

        /// <summary>
        /// <see cref="PerfectScoreStatic"/>
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Needs to stay non-static for reflection")]
        public ScoreFormulatorVariable PerfectScore => PerfectScoreStatic;

        /// <summary>
        /// Distance in meters (m)
        /// </summary>
        public ScoreFormulatorVariable DistanceMeters { get; private set; } = ScoreFormulatorVariable.False;
        /// <summary>
        /// Distance in kilometers (km)
        /// </summary>
        public ScoreFormulatorVariable DistanceKilometers => new(DistanceMeters / 1000D);
        /// <summary>
        /// Distance in feet (ft)
        /// </summary>
        public ScoreFormulatorVariable DistanceFeet => new(DistanceMeters * 3.28084D);
        /// <summary>
        /// Distance in miles (mi)
        /// </summary>
        public ScoreFormulatorVariable DistanceMiles => new(DistanceMeters * 0.000621371D);
        /// <summary>
        /// Time taken in milliseconds (ms)
        /// </summary>
        public ScoreFormulatorVariable Time { get; private set; } = ScoreFormulatorVariable.False;
        /// <summary>
        /// Country streak after the guess
        /// </summary>
        public ScoreFormulatorVariable Streak { get; private set; } = ScoreFormulatorVariable.False;
        /// <summary>
        /// Guess number within the round
        /// </summary>
        public ScoreFormulatorVariable GuesserNumber { get; private set; } = ScoreFormulatorVariable.False;
        /// <summary>
        /// Guesses made by player in the round
        /// </summary>
        public ScoreFormulatorVariable GuessesMade { get; private set; } = ScoreFormulatorVariable.False;
        /// <summary>
        /// Round number within game
        /// </summary>
        public ScoreFormulatorVariable RoundNumber { get; private set; } = ScoreFormulatorVariable.False;
        /// <summary>
        /// Default score calculation result (out of 5000)
        /// </summary>
        public ScoreFormulatorVariable DefaultScore { get; private set; } = ScoreFormulatorVariable.False;

        /// <summary>
        /// If <paramref name="case"/> then returns <paramref name="success"/>, otherwise <see cref="ScoreFormulatorVariable.False"/>
        /// </summary>
        /// <param name="case"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Case(ScoreFormulatorVariable @case, ScoreFormulatorVariable success)
        {
            return Case(@case, success, ScoreFormulatorVariable.False);
        }

        /// <summary>
        /// If <paramref name="case"/> is then returns <paramref name="success"/>, otherwise <paramref name="fail"/>
        /// </summary>
        /// <param name="case"></param>
        /// <param name="success"></param>
        /// <param name="fail"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Case(ScoreFormulatorVariable @case, ScoreFormulatorVariable success, ScoreFormulatorVariable fail)
        {
            return Case((bool)@case, success, fail);
        }

        /// <summary>
        /// If <paramref name="case"/> then returns <paramref name="success"/>, otherwise <see cref="ScoreFormulatorVariable.False"/>
        /// </summary>
        /// <param name="case"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Case(bool @case, ScoreFormulatorVariable success)
        {
            return Case(@case, success, ScoreFormulatorVariable.False);
        }

        /// <summary>
        /// If <paramref name="case"/> is then returns <paramref name="success"/>, otherwise <paramref name="fail"/>
        /// </summary>
        /// <param name="case"></param>
        /// <param name="success"></param>
        /// <param name="fail"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Case(bool @case, ScoreFormulatorVariable success, ScoreFormulatorVariable fail)
        {
            return @case ? success : fail;
        }

        /// <summary>
        /// If <paramref name="case"/> is then returns <paramref name="success"/>, otherwise <paramref name="fail"/>
        /// </summary>
        /// <param name="case"></param>
        /// <param name="success"></param>
        /// <param name="fail"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Case(object @case, object success, object fail)
        {
            return Case(new(@case), new(success), new(fail));
        }

        /// <summary>
        /// Minimum <see cref="ScoreFormulatorVariable.Value"/> of given <paramref name="vars"/>
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Min(params ScoreFormulatorVariable[] vars)
        {
            return vars == null ? ScoreFormulatorVariable.False : vars.Length > 0 ? vars.Min() : ScoreFormulatorVariable.False;
        }

        /// <summary>
        /// Minimum <see cref="ScoreFormulatorVariable.Value"/> of given <paramref name="vars"/>
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Min(params object[] vars)
        {
            return Min(vars.Select(o => new ScoreFormulatorVariable(o)).ToArray());
        }

        /// <summary>
        /// Maximum <see cref="ScoreFormulatorVariable.Value"/> of given <paramref name="vars"/>
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Max(params ScoreFormulatorVariable[] vars)
        {
            return vars == null ? ScoreFormulatorVariable.False : vars.Length > 0 ? vars.Max() : ScoreFormulatorVariable.False;
        }

        /// <summary>
        /// Maximum <see cref="ScoreFormulatorVariable.Value"/> of given <paramref name="vars"/>
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Max(params object[] vars)
        {
            return Max(vars.Select(o => new ScoreFormulatorVariable(o)).ToArray());
        }

        /// <summary>
        /// Round <paramref name="value"/> to closest integer
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Round(ScoreFormulatorVariable value)
        {
            return Round(value, ScoreFormulatorVariable.False);
        }

        /// <summary>
        /// Round <paramref name="value"/> to closest integer
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Round(object value)
        {
            return Round(new(value), ScoreFormulatorVariable.False);
        }

        /// <summary>
        /// Round <paramref name="value"/> to <paramref name="digits"/> decimals
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Round(ScoreFormulatorVariable value, ScoreFormulatorVariable digits)
        {
            if (value == null)
            {
                return ScoreFormulatorVariable.False;
            }

            digits ??= ScoreFormulatorVariable.False;

            return new(Math.Round(value.Value, digits.Value.CastAsInt()));
        }

        /// <summary>
        /// Round <paramref name="value"/> to <paramref name="digits"/> decimals
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Round(object value, object digits)
        {
            return Round(new(value), new(digits));
        }

        /// <summary>
        /// Return one of <paramref name="vars"/> or <see cref="ScoreFormulatorVariable.False"/>
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable RandomPick(params ScoreFormulatorVariable[] vars)
        {
            return vars == null
                ? ScoreFormulatorVariable.False
                : vars.Length > 0 ? vars[random.Next(vars.Length)] : ScoreFormulatorVariable.False;
        }

        /// <summary>
        /// Return one of <paramref name="vars"/> or <see cref="ScoreFormulatorVariable.False"/>
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable RandomPick(params object[] vars)
        {
            return RandomPick(vars.Select(o => new ScoreFormulatorVariable(o)).ToArray());
        }

        /// <summary>
        /// Return a double in range <c>[0, 1]</c>
        /// </summary>
        /// <returns></returns>
        public static ScoreFormulatorVariable Random()
        {
            return Random(ScoreFormulatorVariable.False, ScoreFormulatorVariable.True);
        }

        /// <summary>
        /// Return a double in range <c>[0, <paramref name="max"/>]</c>
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Random(ScoreFormulatorVariable max)
        {
            return Random(ScoreFormulatorVariable.False, max);
        }

        /// <summary>
        /// Return a double in range <c>[0, <paramref name="max"/>]</c>
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Random(object max)
        {
            return Random(ScoreFormulatorVariable.False, new(max));
        }

        /// <summary>
        /// Return a double in range <c>[<paramref name="min"/>, <paramref name="max"/>]</c>
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Random(ScoreFormulatorVariable min, ScoreFormulatorVariable max)
        {
            if (max == null)
            {
                return Random(min);
            }
            else if (min == null)
            {
                return ScoreFormulatorVariable.False;
            }

            return max >= min ? new(random.GetDouble(min.Value, max.Value)) : new(random.GetDouble(max.Value, min.Value));
        }

        /// <summary>
        /// Return a double in range <c>[<paramref name="min"/>, <paramref name="max"/>]</c>
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Random(object min, object max)
        {
            return Random(new(min), new(max));
        }

        /// <summary>
        /// Returns <c><see cref="ScoreFormulatorVariable.False"/> ^ <see cref="ScoreFormulatorVariable.True"/></c>
        /// </summary>
        /// <returns></returns>
        public static ScoreFormulatorVariable Pow()
        {
            return Pow(ScoreFormulatorVariable.False, ScoreFormulatorVariable.True);
        }

        /// <summary>
        /// Returns <c><paramref name="base"/> ^ <see cref="ScoreFormulatorVariable.True"/></c>
        /// </summary>
        /// <param name="base"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Pow(ScoreFormulatorVariable @base)
        {
            return Pow(@base, ScoreFormulatorVariable.True);
        }

        /// <summary>
        /// Returns <c><paramref name="base"/> ^ 1</c>
        /// </summary>
        /// <param name="base"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Pow(object @base)
        {
            return Pow(new(@base));
        }

        /// <summary>
        /// Returns <c><paramref name="base"/> ^ <paramref name="power"/></c>
        /// </summary>
        /// <param name="base"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Pow(ScoreFormulatorVariable @base, ScoreFormulatorVariable power)
        {
            return new(Math.Pow(@base, power));
        }

        /// <summary>
        /// Returns <c><paramref name="base"/> ^ <paramref name="power"/></c>
        /// </summary>
        /// <param name="base"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Pow(object @base, object power)
        {
            return Pow(new(@base), new(power));
        }

        /// <summary>
        /// Returns <see cref="ScoreFormulatorVariable.False"/>
        /// </summary>
        /// <returns></returns>
        public static ScoreFormulatorVariable Abs()
        {
            return Abs(ScoreFormulatorVariable.False);
        }

        /// <summary>
        /// Returns absolute value of <paramref name="value"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Abs(ScoreFormulatorVariable value)
        {
            return value == null ? ScoreFormulatorVariable.False : value.Value < 0 ? new(Math.Abs(value.Value)) : value;
        }

        /// <summary>
        /// Returns absolute value of <paramref name="value"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable Abs(object value)
        {
            return Abs(new(value));
        }
    }
}
