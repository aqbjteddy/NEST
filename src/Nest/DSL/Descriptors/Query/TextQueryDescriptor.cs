﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq.Expressions;

namespace Nest
{
  public class TextQueryDescriptor<T>  where T : class
	{
		[JsonProperty(PropertyName = "type")]
		internal virtual string _Type { get { return null; } }

		[JsonProperty(PropertyName = "query")]
		internal string _Query { get; set; }

		[JsonProperty(PropertyName = "analyzer")]
		internal string _Analyzer { get; set; }

		[JsonProperty(PropertyName = "fuzziness")]
		internal double? _Fuzziness { get; set; }

		[JsonProperty(PropertyName = "prefix_length")]
		internal int? _PrefixLength { get; set; }

		[JsonProperty(PropertyName = "max_expansions")]
		internal int? _MaxExpansions { get; set; }

		[JsonProperty(PropertyName = "slop")]
		internal int? _Slop { get; set; }

		[JsonProperty(PropertyName = "operator")]
		[JsonConverter(typeof(StringEnumConverter))]
		internal Operator? _Operator { get; set; }

		internal string _Field { get; set; }
		public TextQueryDescriptor<T> OnField(string field)
		{
			this._Field = field;
			return this;
		}
		public TextQueryDescriptor<T> OnField(Expression<Func<T, object>> objectPath)
		{
			var fieldName = ElasticClient.PropertyNameResolver.Resolve(objectPath);
			return this.OnField(fieldName);
		}

		public TextQueryDescriptor<T> QueryString(string queryString)
		{
			queryString.ThrowIfNullOrEmpty("queryString");
			this._Query = queryString;
			return this;
		}
		public TextQueryDescriptor<T> Analyzer(string analyzer)
		{
			analyzer.ThrowIfNullOrEmpty("analyzer");
			this._Analyzer = analyzer;
			return this;
		}
		public TextQueryDescriptor<T> Fuzziness(double fuzziness)
		{
			fuzziness.ThrowIfNull("fuzziness");
			this._Fuzziness = fuzziness;
			return this;
		}
		public TextQueryDescriptor<T> PrefixLength(int prefixLength)
		{
			prefixLength.ThrowIfNull("prefixLength");
			this._PrefixLength = prefixLength;
			return this;
		}
		public TextQueryDescriptor<T> MaxExpansions(int maxExpansions)
		{
			maxExpansions.ThrowIfNull("maxExpansions");
			this._MaxExpansions = maxExpansions;
			return this;
		}
		public TextQueryDescriptor<T> Slop(int slop)
		{
			slop.ThrowIfNull("slop");
			this._Slop = slop;
			return this;
		}
		public TextQueryDescriptor<T> Operator(Operator op)
		{
			this._Operator = op;
			return this;
		}
	}
}
