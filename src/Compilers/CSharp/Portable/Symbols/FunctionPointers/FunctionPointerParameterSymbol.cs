﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.
#nullable enable

using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.CSharp.Symbols
{
    internal sealed class FunctionPointerParameterSymbol : ParameterSymbol
    {
        private readonly FunctionPointerMethodSymbol _containingSymbol;

        public FunctionPointerParameterSymbol(TypeWithAnnotations typeWithAnnotations, RefKind refKind, int ordinal, FunctionPointerMethodSymbol containingSymbol, ImmutableArray<CustomModifier> refCustomModifiers, ImmutableArray<Location> locations)
        {
            TypeWithAnnotations = typeWithAnnotations;
            RefKind = refKind;
            Ordinal = ordinal;
            _containingSymbol = containingSymbol;
            RefCustomModifiers = refCustomModifiers;
            Locations = locations;
        }

        public override TypeWithAnnotations TypeWithAnnotations { get; }
        public override RefKind RefKind { get; }
        public override int Ordinal { get; }
        public override Symbol ContainingSymbol => _containingSymbol;
        public override ImmutableArray<CustomModifier> RefCustomModifiers { get; }
        public override ImmutableArray<Location> Locations { get; }
        public override ImmutableArray<SyntaxReference> DeclaringSyntaxReferences => GetDeclaringSyntaxReferenceHelper<ParameterSyntax>(Locations);

        public override bool Equals(Symbol other, TypeCompareKind compareKind)
        {
            if (!(other is FunctionPointerParameterSymbol param))
            {
                return false;
            }

            return Equals(param, compareKind, isValueTypeOverride: null);
        }

        internal bool Equals(FunctionPointerParameterSymbol other, TypeCompareKind compareKind, IReadOnlyDictionary<TypeParameterSymbol, bool>? isValueTypeOverride)
            => EqualsContainingVerified(other, compareKind, isValueTypeOverride)
               && _containingSymbol.Equals(other._containingSymbol, compareKind, isValueTypeOverride);

        internal bool EqualsContainingVerified(FunctionPointerParameterSymbol other, TypeCompareKind compareKind, IReadOnlyDictionary<TypeParameterSymbol, bool>? isValueTypeOverride)
            => RefKind == other.RefKind
               && Ordinal == other.Ordinal
               && RefCustomModifiers.Equals(other.RefCustomModifiers)
               && TypeWithAnnotations.Equals(other.TypeWithAnnotations, compareKind, isValueTypeOverride);

        public override int GetHashCode()
        {
            return Hash.Combine(_containingSymbol.GetHashCode(), GetHashCodeNoContaining());
        }

        internal int GetHashCodeNoContaining()
            => Hash.Combine(TypeWithAnnotations.GetHashCode(),
               Hash.Combine(RefKind.GetHashCode(),
               Hash.Combine(Ordinal.GetHashCode(),
               Hash.CombineValues(RefCustomModifiers))));

        public override bool IsDiscard => false;
        public override bool IsParams => false;
        internal override MarshalPseudoCustomAttributeData? MarshallingInformation => null;
        internal override bool IsMetadataOptional => false;
        internal override bool IsMetadataIn => RefKind == RefKind.In;
        internal override bool IsMetadataOut => RefKind == RefKind.Out;
        internal override ConstantValue? ExplicitDefaultConstantValue => null;
        internal override bool IsIDispatchConstant => false;
        internal override bool IsIUnknownConstant => false;
        internal override bool IsCallerFilePath => false;
        internal override bool IsCallerLineNumber => false;
        internal override bool IsCallerMemberName => false;
        internal override FlowAnalysisAnnotations FlowAnalysisAnnotations => FlowAnalysisAnnotations.None;
        internal override ImmutableHashSet<string> NotNullIfParameterNotNull => ImmutableHashSet<string>.Empty;
    }
}
