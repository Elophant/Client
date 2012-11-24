using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Diagnostics.SymbolStore;
using System.Diagnostics.CodeAnalysis;

namespace FluorineFx.Reflection.Lightweight
{
	/// <summary>
	/// A wrapper around the <see cref="ILGenerator"/> class.
	/// </summary>
	/// <seealso cref="System.Reflection.Emit.ILGenerator">ILGenerator Class</seealso>
	class EmitHelper
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EmitHelper"/> class
		/// with the specified <see cref="System.Reflection.Emit.ILGenerator"/>.
		/// </summary>
		/// <param name="ilGenerator">The <see cref="System.Reflection.Emit.ILGenerator"/> to use.</param>
		public EmitHelper(ILGenerator ilGenerator)
		{
			if (ilGenerator == null) throw new ArgumentNullException("ilGenerator");

			_ilGenerator = ilGenerator;
		}

		private readonly ILGenerator _ilGenerator;
		/// <summary>
		/// Gets MSIL generator.
		/// </summary>
		public           ILGenerator  ILGenerator
		{
			get { return _ilGenerator; }
		}

		/// <summary>
		/// Converts the supplied <see cref="EmitHelper"/> to a <see cref="ILGenerator"/>.
		/// </summary>
		/// <param name="emitHelper">The EmitHelper.</param>
		/// <returns>An ILGenerator.</returns>
		public static implicit operator ILGenerator(EmitHelper emitHelper)
		{
			if (emitHelper == null) throw new ArgumentNullException("emitHelper");

			return emitHelper.ILGenerator;
		}

		#region ILGenerator Methods

		/// <summary>
		/// Begins a catch block.
		/// </summary>
		/// <param name="exceptionType">The Type object that represents the exception.</param>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.BeginCatchBlock(Type)">ILGenerator.BeginCatchBlock Method</seealso>
		public EmitHelper BeginCatchBlock(Type exceptionType)
		{
			_ilGenerator.BeginCatchBlock(exceptionType); return this;
		}

		/// <summary>
		/// Begins an exception block for a filtered exception.
		/// </summary>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.BeginExceptFilterBlock">ILGenerator.BeginCatchBlock Method</seealso>
		public EmitHelper BeginExceptFilterBlock()
		{
			_ilGenerator.BeginExceptFilterBlock(); return this;
		}

		/// <summary>
		/// Begins an exception block for a non-filtered exception.
		/// </summary>
		/// <returns>The label for the end of the block.</returns>
		public Label BeginExceptionBlock()
		{
			return _ilGenerator.BeginExceptionBlock();
		}

		/// <summary>
		/// Begins an exception fault block in the Microsoft intermediate language (MSIL) stream.
		/// </summary>
		/// <returns>Current instance of the EmitHelper.</returns>
		public EmitHelper BeginFaultBlock()
		{
			_ilGenerator.BeginFaultBlock(); return this;
		}

		/// <summary>
		/// Begins a finally block in the Microsoft intermediate language (MSIL) instruction stream.
		/// </summary>
		/// <returns>Current instance of the EmitHelper.</returns>
		public EmitHelper BeginFinallyBlock()
		{
			_ilGenerator.BeginFinallyBlock(); return this;
		}

		/// <summary>
		/// Begins a lexical scope.
		/// </summary>
		/// <returns>Current instance of the EmitHelper.</returns>
		public EmitHelper BeginScope()
		{
			_ilGenerator.BeginScope(); return this;
		}

		/// <summary>
		/// Declares a local variable.
		/// </summary>
		/// <param name="localType">The Type of the local variable.</param>
		/// <returns>The declared local variable.</returns>
		public LocalBuilder DeclareLocal(Type localType)
		{
			return _ilGenerator.DeclareLocal(localType);
		}

		/// <summary>
		/// Declares a new label.
		/// </summary>
		/// <returns>Returns a new label that can be used as a token for branching.</returns>
		public Label DefineLabel()
		{
			return _ilGenerator.DefineLabel();
		}

		/// <summary>
		/// Ends an exception block.
		/// </summary>
		/// <returns>Current instance of the EmitHelper.</returns>
		public EmitHelper EndExceptionBlock()
		{
			_ilGenerator.EndExceptionBlock(); return this;
		}

		/// <summary>
		/// Ends a lexical scope.
		/// </summary>
		/// <returns>Current instance of the EmitHelper.</returns>
		public EmitHelper EndScope()
		{
			_ilGenerator.EndScope(); return this;
		}

		/// <summary>
		/// Marks the Microsoft intermediate language (MSIL) stream's current position 
		/// with the given label.
		/// </summary>
		/// <param name="loc">The label for which to set an index.</param>
		/// <returns>Current instance of the EmitHelper.</returns>
		public EmitHelper MarkLabel(Label loc)
		{
			_ilGenerator.MarkLabel(loc); return this;
		}

		/// <summary>
		/// Marks a sequence point in the Microsoft intermediate language (MSIL) stream.
		/// </summary>
		/// <param name="document">The document for which the sequence point is being defined.</param>
		/// <param name="startLine">The line where the sequence point begins.</param>
		/// <param name="startColumn">The column in the line where the sequence point begins.</param>
		/// <param name="endLine">The line where the sequence point ends.</param>
		/// <param name="endColumn">The column in the line where the sequence point ends.</param>
		/// <returns>Current instance of the EmitHelper.</returns>
		public EmitHelper MarkSequencePoint(
			ISymbolDocumentWriter document,
			int startLine,
			int startColumn,
			int endLine,
			int endColumn)
		{
			_ilGenerator.MarkSequencePoint(document, startLine, startColumn, endLine, endColumn);
			return this;
		}

		/// <summary>
		/// Emits an instruction to throw an exception.
		/// </summary>
		/// <param name="exceptionType">The class of the type of exception to throw.</param>
		/// <returns>Current instance of the EmitHelper.</returns>
		public EmitHelper ThrowException(Type exceptionType)
		{
			_ilGenerator.ThrowException(exceptionType); return this;
		}

		/// <summary>
		/// Specifies the namespace to be used in evaluating locals and watches for 
		/// the current active lexical scope.
		/// </summary>
		/// <param name="namespaceName">The namespace to be used in evaluating locals and watches for the current active lexical scope.</param>
		/// <returns>Current instance of the EmitHelper.</returns>
		public EmitHelper UsingNamespace(string namespaceName)
		{
			_ilGenerator.UsingNamespace(namespaceName); return this;
		}

		#endregion

		#region Emit Wrappers

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Add"/>) that
		/// adds two values and pushes the result onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Add">OpCodes.Add</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper add
		{
			get { _ilGenerator.Emit(OpCodes.Add); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Add_Ovf"/>) that
		/// adds two integers, performs an overflow check, and pushes the result onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Add_Ovf">OpCodes.Add_Ovf</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper add_ovf
		{
			get { _ilGenerator.Emit(OpCodes.Add_Ovf); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Add_Ovf_Un"/>) that
		/// adds two unsigned integer values, performs an overflow check, and pushes the result onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Add_Ovf_Un">OpCodes.Add_Ovf_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper add_ovf_un
		{
			get { _ilGenerator.Emit(OpCodes.Add_Ovf_Un); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.And"/>) that
		/// computes the bitwise AND of two values and pushes the result onto the evalution stack.
		/// </summary>
		/// <seealso cref="OpCodes.And">OpCodes.And</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper and
		{
			get { _ilGenerator.Emit(OpCodes.And); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Arglist"/>) that
		/// returns an unmanaged pointer to the argument list of the current method.
		/// </summary>
		/// <seealso cref="OpCodes.Arglist">OpCodes.Arglist</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper arglist
		{
			get { _ilGenerator.Emit(OpCodes.Arglist); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Beq"/>, label) that
		/// transfers control to a target instruction if two values are equal.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Beq">OpCodes.Beq</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper beq(Label label)
		{
			_ilGenerator.Emit(OpCodes.Beq, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Beq_S"/>, label) that
		/// transfers control to a target instruction (short form) if two values are equal.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Beq_S">OpCodes.Beq_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper beq_s(Label label)
		{
			_ilGenerator.Emit(OpCodes.Beq_S, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Bge"/>, label) that
		/// transfers control to a target instruction if the first value is greater than or equal to the second value.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Bge">OpCodes.Bge</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper bge(Label label)
		{
			_ilGenerator.Emit(OpCodes.Bge, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Bge_S"/>, label) that
		/// transfers control to a target instruction (short form) 
		/// if the first value is greater than or equal to the second value.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Bge_S">OpCodes.Bge_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper bge_s(Label label)
		{
			_ilGenerator.Emit(OpCodes.Bge_S, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Bge_Un"/>, label) that
		/// transfers control to a target instruction if the the first value is greather than the second value,
		/// when comparing unsigned integer values or unordered float values.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Bge_Un">OpCodes.Bge_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper bge_un(Label label)
		{
			_ilGenerator.Emit(OpCodes.Bge_Un, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Bge_Un_S"/>, label) that
		/// transfers control to a target instruction (short form) if if the the first value is greather than the second value,
		/// when comparing unsigned integer values or unordered float values.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Bge_Un_S">OpCodes.Bge_Un_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper bge_un_s(Label label)
		{
			_ilGenerator.Emit(OpCodes.Bge_Un_S, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Bgt"/>, label) that
		/// transfers control to a target instruction if the first value is greater than the second value.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Bgt">OpCodes.Bgt</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper bgt(Label label)
		{
			_ilGenerator.Emit(OpCodes.Bgt, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Bgt_S"/>, label) that
		/// transfers control to a target instruction (short form) if the first value is greater than the second value.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Bgt_S">OpCodes.Bgt_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper bgt_s(Label label)
		{
			_ilGenerator.Emit(OpCodes.Bgt_S, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Bgt_Un"/>, label) that
		/// transfers control to a target instruction if the first value is greater than the second value,
		/// when comparing unsigned integer values or unordered float values.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Bgt_Un">OpCodes.Bgt_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper bgt_un(Label label)
		{
			_ilGenerator.Emit(OpCodes.Bgt_Un, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Bgt_Un_S"/>, label) that
		/// transfers control to a target instruction (short form) if the first value is greater than the second value,
		/// when comparing unsigned integer values or unordered float values.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Bgt_Un_S">OpCodes.Bgt_Un_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper bgt_un_s(Label label)
		{
			_ilGenerator.Emit(OpCodes.Bgt_Un_S, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ble"/>, label) that
		/// transfers control to a target instruction if the first value is less than or equal to the second value.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Ble">OpCodes.Ble</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper ble(Label label)
		{
			_ilGenerator.Emit(OpCodes.Ble, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ble_S"/>, label) that
		/// transfers control to a target instruction (short form) if the first value is less than or equal to the second value.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Ble_S">OpCodes.Ble_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper ble_s(Label label)
		{
			_ilGenerator.Emit(OpCodes.Ble_S, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ble_Un"/>, label) that
		/// transfers control to a target instruction if the first value is less than or equal to the second value,
		/// when comparing unsigned integer values or unordered float values.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Ble_Un">OpCodes.Ble_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper ble_un(Label label)
		{
			_ilGenerator.Emit(OpCodes.Ble_Un, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ble_Un_S"/>, label) that
		/// transfers control to a target instruction (short form) if the first value is less than or equal to the second value,
		/// when comparing unsigned integer values or unordered float values.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Ble_Un_S">OpCodes.Ble_Un_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper ble_un_s(Label label)
		{
			_ilGenerator.Emit(OpCodes.Ble_Un_S, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Blt"/>, label) that
		/// transfers control to a target instruction if the first value is less than the second value.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Blt">OpCodes.Blt</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper blt(Label label)
		{
			_ilGenerator.Emit(OpCodes.Blt, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Blt_S"/>, label) that
		/// transfers control to a target instruction (short form) if the first value is less than the second value.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Blt_S">OpCodes.Blt_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper blt_s(Label label)
		{
			_ilGenerator.Emit(OpCodes.Blt_S, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Blt_Un"/>, label) that
		/// transfers control to a target instruction if the first value is less than the second value,
		/// when comparing unsigned integer values or unordered float values.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Blt_Un">OpCodes.Blt_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper blt_un(Label label)
		{
			_ilGenerator.Emit(OpCodes.Blt_Un, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Blt_Un_S"/>, label) that
		/// transfers control to a target instruction (short form) if the first value is less than the second value,
		/// when comparing unsigned integer values or unordered float values.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Blt_Un_S">OpCodes.Blt_Un_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper blt_un_s(Label label)
		{
			_ilGenerator.Emit(OpCodes.Blt_Un_S, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Bne_Un"/>, label) that
		/// transfers control to a target instruction when two unsigned integer values or unordered float values are not equal.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Bne_Un">OpCodes.Bne_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper bne_un(Label label)
		{
			_ilGenerator.Emit(OpCodes.Bne_Un, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Bne_Un_S"/>, label) that
		/// transfers control to a target instruction (short form) 
		/// when two unsigned integer values or unordered float values are not equal.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Bne_Un_S">OpCodes.Bne_Un_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper bne_un_s(Label label)
		{
			_ilGenerator.Emit(OpCodes.Bne_Un_S, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Box"/>, type) that
		/// converts a value type to an object reference.
		/// </summary>
		/// <param name="type">A Type</param>
		/// <seealso cref="OpCodes.Box">OpCodes.Box</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Type)">ILGenerator.Emit</seealso>
		public EmitHelper box(Type type)
		{
			_ilGenerator.Emit(OpCodes.Box, type); return this;
		}

		/// <summary>
		/// Converts a value type to an object reference if the value is a value type.
		/// </summary>
		/// <param name="type">A Type</param>
		/// <seealso cref="OpCodes.Box">OpCodes.Box</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Type)">ILGenerator.Emit</seealso>
		public EmitHelper boxIfValueType(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			return type.IsValueType? box(type): this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Br"/>, label) that
		/// unconditionally transfers control to a target instruction. 
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Br">OpCodes.Br</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper br(Label label)
		{
			_ilGenerator.Emit(OpCodes.Br, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Break"/>) that
		/// signals the Common Language Infrastructure (CLI) to inform the debugger that a break point has been tripped.
		/// </summary>
		/// <seealso cref="OpCodes.Break">OpCodes.Break</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper @break
		{
			get { _ilGenerator.Emit(OpCodes.Break); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Brfalse"/>, label) that
		/// transfers control to a target instruction if value is false, a null reference (Nothing in Visual Basic), or zero.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Brfalse">OpCodes.Brfalse</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper brfalse(Label label)
		{
			_ilGenerator.Emit(OpCodes.Brfalse, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Brfalse_S"/>, label) that
		/// transfers control to a target instruction if value is false, a null reference, or zero. 
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Brfalse_S">OpCodes.Brfalse_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper brfalse_s(Label label)
		{
			_ilGenerator.Emit(OpCodes.Brfalse_S, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Brtrue"/>, label) that
		/// transfers control to a target instruction if value is true, not null, or non-zero.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Brtrue">OpCodes.Brtrue</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper brtrue(Label label)
		{
			_ilGenerator.Emit(OpCodes.Brtrue, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Brtrue_S"/>, label) that
		/// transfers control to a target instruction (short form) if value is true, not null, or non-zero.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Brtrue_S">OpCodes.Brtrue_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper brtrue_s(Label label)
		{
			_ilGenerator.Emit(OpCodes.Brtrue_S, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Br_S"/>, label) that
		/// unconditionally transfers control to a target instruction (short form).
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Br_S">OpCodes.Br_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper br_s(Label label)
		{
			_ilGenerator.Emit(OpCodes.Br_S, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Call"/>, methodInfo) that
		/// calls the method indicated by the passed method descriptor.
		/// </summary>
		/// <param name="methodInfo">The method to be called.</param>
		/// <seealso cref="OpCodes.Call">OpCodes.Call</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,MethodInfo)">ILGenerator.Emit</seealso>
		public EmitHelper call(MethodInfo methodInfo)
		{
			_ilGenerator.Emit(OpCodes.Call, methodInfo); return this;
		}

		public EmitHelper call(ConstructorInfo constructorInfo)
		{
			_ilGenerator.Emit(OpCodes.Call, constructorInfo); return this;
		}

		/// <summary>
		/// Calls ILGenerator.EmitCall(<see cref="OpCodes.Call"/>, methodInfo, optionalParameterTypes) that
		/// calls the method indicated by the passed method descriptor.
		/// </summary>
		/// <param name="methodInfo">The method to be called.</param>
		/// <param name="optionalParameterTypes">The types of the optional arguments if the method is a varargs method.</param>
		/// <seealso cref="OpCodes.Call">OpCodes.Call</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.EmitCall(OpCode,MethodInfo,Type[])">ILGenerator.EmitCall</seealso>
		public EmitHelper call(MethodInfo methodInfo, Type[] optionalParameterTypes)
		{
			_ilGenerator.EmitCall(OpCodes.Call, methodInfo, optionalParameterTypes); return this;
		}

		public EmitHelper call(Type type, string methodName, params Type[] parameterTypes)
		{
			if (type == null) throw new ArgumentNullException("type");

			MethodInfo mi = type.GetMethod(methodName, parameterTypes);

			return call(mi);
		}

		public EmitHelper call(Type type, string methodName, BindingFlags flags, params Type[] parameterTypes)
		{
			if (type == null) throw new ArgumentNullException("type");

			MethodInfo mi = type.GetMethod(methodName, flags, null, parameterTypes, null);

			return call(mi);
		}

		/// <summary>
		/// Calls ILGenerator.EmitCalli(<see cref="OpCodes.Calli"/>, CallingConvention, Type, Type[]) that
		/// calls the method indicated on the evaluation stack (as a pointer to an entry point) 
		/// with arguments described by a calling convention using an unmanaged calling convention.
		/// </summary>
		/// <param name="unmanagedCallConv">The unmanaged calling convention to be used.</param>
		/// <param name="returnType">The Type of the result.</param>
		/// <param name="parameterTypes">The types of the required arguments to the instruction.</param>
		/// <seealso cref="OpCodes.Calli">OpCodes.Calli</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.EmitCalli(OpCode,CallingConvention,Type,Type[])">ILGenerator.EmitCalli</seealso>
		public EmitHelper calli(CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
		{
			_ilGenerator.EmitCalli(OpCodes.Calli, unmanagedCallConv, returnType, parameterTypes); return this;
		}

		/// <summary>
		/// Calls ILGenerator.EmitCalli(<see cref="OpCodes.Calli"/>, CallingConventions, Type, Type[], Type[]) that
		/// calls the method indicated on the evaluation stack (as a pointer to an entry point)
		/// with arguments described by a calling convention using a managed calling convention.
		/// </summary>
		/// <param name="callingConvention">The managed calling convention to be used.</param>
		/// <param name="returnType">The Type of the result.</param>
		/// <param name="parameterTypes">The types of the required arguments to the instruction.</param>
		/// <param name="optionalParameterTypes">The types of the optional arguments for vararg calls.</param>
		/// <seealso cref="OpCodes.Calli">OpCodes.Calli</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.EmitCalli(OpCode,CallingConventions,Type,Type[],Type[])">ILGenerator.EmitCalli</seealso>
		public EmitHelper calli(CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
		{
			_ilGenerator.EmitCalli(OpCodes.Calli, callingConvention, returnType, parameterTypes, optionalParameterTypes);
			return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Callvirt"/>, methodInfo) that
		/// calls a late-bound method on an object, pushing the return value onto the evaluation stack.
		/// </summary>
		/// <param name="methodInfo">The method to be called.</param>
		/// <seealso cref="OpCodes.Callvirt">OpCodes.Callvirt</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,MethodInfo)">ILGenerator.Emit</seealso>
		public EmitHelper callvirt(MethodInfo methodInfo)
		{
			_ilGenerator.Emit(OpCodes.Callvirt, methodInfo); return this;
		}

		/// <summary>
		/// Calls ILGenerator.EmitCall(<see cref="OpCodes.Callvirt"/>, methodInfo, optionalParameterTypes) that
		/// calls a late-bound method on an object, pushing the return value onto the evaluation stack.
		/// </summary>
		/// <param name="methodInfo">The method to be called.</param>
		/// <param name="optionalParameterTypes">The types of the optional arguments if the method is a varargs method.</param>
		/// <seealso cref="OpCodes.Callvirt">OpCodes.Callvirt</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.EmitCall(OpCode,MethodInfo,Type[])">ILGenerator.EmitCall</seealso>
		public EmitHelper callvirt(MethodInfo methodInfo, Type[] optionalParameterTypes)
		{
			_ilGenerator.EmitCall(OpCodes.Callvirt, methodInfo, optionalParameterTypes); return this;
		}

		/// <summary>
		/// Calls ILGenerator.EmitCall(<see cref="OpCodes.Callvirt"/>, methodInfo, optionalParameterTypes) that
		/// calls a late-bound method on an object, pushing the return value onto the evaluation stack.
		/// </summary>
		/// <param name="methodName">The method to be called.</param>
		/// <param name="type">The declaring type of the method.</param>
		/// <param name="parameterTypes">The types of the optional arguments if the method is a varargs method.</param>
		/// <seealso cref="OpCodes.Callvirt">OpCodes.Callvirt</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.EmitCall(OpCode,MethodInfo,Type[])">ILGenerator.EmitCall</seealso>
		public EmitHelper callvirt(Type type, string methodName, params Type[] parameterTypes)
		{
			if (type == null) throw new ArgumentNullException("type");

			MethodInfo mi = type.GetMethod(methodName, parameterTypes);

			return callvirt(mi);
		}

		public EmitHelper callvirt(Type type, string methodName, BindingFlags bindingAttr, params Type[] optionalParameterTypes)
		{
			MethodInfo methodInfo = 
				optionalParameterTypes == null?
					type.GetMethod(methodName, bindingAttr):
					type.GetMethod(methodName, bindingAttr, null, optionalParameterTypes, null);

			return callvirt(methodInfo, null);
		}

		public EmitHelper callvirt(Type type, string methodName, BindingFlags bindingAttr)
		{
			return callvirt(type, methodName, bindingAttr, null);
		}

		public EmitHelper callvirtNoGenerics(Type type, string methodName, params Type[] parameterTypes)
		{
			MethodInfo method = type.GetMethod(
				methodName,
				BindingFlags.Instance | BindingFlags.Public,
				GenericBinder.NonGeneric,
				parameterTypes, null);

			return callvirt(method, parameterTypes.Length == 0? null: parameterTypes);
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Castclass"/>, type) that
		/// attempts to cast an object passed by reference to the specified class.
		/// </summary>
		/// <param name="type">A Type</param>
		/// <seealso cref="OpCodes.Castclass">OpCodes.Castclass</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Type)">ILGenerator.Emit</seealso>
		public EmitHelper castclass(Type type)
		{
			_ilGenerator.Emit(OpCodes.Castclass, type); return this;
		}

		/// <summary>
		/// Attempts to cast an object passed by reference to the specified class 
		/// or to unbox if the type is a value type.
		/// </summary>
		/// <param name="type">A Type</param>
		public EmitHelper castType(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			return type.IsValueType? unbox(type): castclass(type);
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ceq"/>) that
		/// compares two values. If they are equal, the integer value 1 (int32) is pushed onto the evaluation stack;
		/// otherwise 0 (int32) is pushed onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Ceq">OpCodes.Ceq</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ceq
		{
			get { _ilGenerator.Emit(OpCodes.Ceq); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Cgt"/>) that
		/// compares two values. If the first value is greater than the second,
		/// the integer value 1 (int32) is pushed onto the evaluation stack;
		/// otherwise 0 (int32) is pushed onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Cgt">OpCodes.Cgt</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper cgt
		{
			get { _ilGenerator.Emit(OpCodes.Cgt); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Cgt_Un"/>) that
		/// compares two unsigned or unordered values.
		/// If the first value is greater than the second, the integer value 1 (int32) is pushed onto the evaluation stack;
		/// otherwise 0 (int32) is pushed onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Cgt_Un">OpCodes.Cgt_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper cgt_un
		{
			get { _ilGenerator.Emit(OpCodes.Cgt_Un); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ckfinite"/>) that
		/// throws ArithmeticException if value is not a finite number.
		/// </summary>
		/// <seealso cref="OpCodes.Ckfinite">OpCodes.Ckfinite</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ckfinite
		{
			get { _ilGenerator.Emit(OpCodes.Ckfinite); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Clt"/>) that
		/// compares two values. If the first value is less than the second,
		/// the integer value 1 (int32) is pushed onto the evaluation stack;
		/// otherwise 0 (int32) is pushed onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Clt">OpCodes.Clt</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper clt
		{
			get { _ilGenerator.Emit(OpCodes.Clt); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Clt_Un"/>) that
		/// compares the unsigned or unordered values value1 and value2.
		/// If value1 is less than value2, then the integer value 1 (int32) is pushed onto the evaluation stack;
		/// otherwise 0 (int32) is pushed onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Clt_Un">OpCodes.Clt_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper clt_un
		{
			get { _ilGenerator.Emit(OpCodes.Clt_Un); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_I"/>) that
		/// converts the value on top of the evaluation stack to natural int.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_I">OpCodes.Conv_I</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_i
		{
			get { _ilGenerator.Emit(OpCodes.Conv_I); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_I1"/>) that
		/// converts the value on top of the evaluation stack to int8, then extends (pads) it to int32.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_I1">OpCodes.Conv_I1</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_i1
		{
			get { _ilGenerator.Emit(OpCodes.Conv_I1); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_I2"/>) that
		/// converts the value on top of the evaluation stack to int16, then extends (pads) it to int32.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_I2">OpCodes.Conv_I2</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_i2
		{
			get { _ilGenerator.Emit(OpCodes.Conv_I2); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_I4"/>) that
		/// converts the value on top of the evaluation stack to int32.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_I4">OpCodes.Conv_I4</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_i4
		{
			get { _ilGenerator.Emit(OpCodes.Conv_I4); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_I8"/>) that
		/// converts the value on top of the evaluation stack to int64.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_I8">OpCodes.Conv_I8</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_i8
		{
			get { _ilGenerator.Emit(OpCodes.Conv_I8); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_Ovf_I"/>) that
		/// converts the signed value on top of the evaluation stack to signed natural int,
		/// throwing OverflowException on overflow.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_Ovf_I">OpCodes.Conv_Ovf_I</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_ovf_i
		{
			get { _ilGenerator.Emit(OpCodes.Conv_Ovf_I); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_Ovf_I1"/>) that
		/// converts the signed value on top of the evaluation stack to signed int8 and extends it to int32,
		/// throwing OverflowException on overflow.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_Ovf_I1">OpCodes.Conv_Ovf_I1</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_ovf_i1
		{
			get { _ilGenerator.Emit(OpCodes.Conv_Ovf_I1); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_Ovf_I1_Un"/>) that
		/// converts the unsigned value on top of the evaluation stack to signed int8 and extends it to int32,
		/// throwing OverflowException on overflow.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_Ovf_I1_Un">OpCodes.Conv_Ovf_I1_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_ovf_i1_un
		{
			get { _ilGenerator.Emit(OpCodes.Conv_Ovf_I1_Un); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_Ovf_I2"/>) that
		/// converts the signed value on top of the evaluation stack to signed int16 and extending it to int32,
		/// throwing OverflowException on overflow.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_Ovf_I2">OpCodes.Conv_Ovf_I2</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_ovf_i2
		{
			get { _ilGenerator.Emit(OpCodes.Conv_Ovf_I2); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_Ovf_I2_Un"/>) that
		/// converts the unsigned value on top of the evaluation stack to signed int16 and extends it to int32,
		/// throwing OverflowException on overflow.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_Ovf_I2_Un">OpCodes.Conv_Ovf_I2_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_ovf_i2_un
		{
			get { _ilGenerator.Emit(OpCodes.Conv_Ovf_I2_Un); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_Ovf_I4"/>) that
		/// converts the signed value on top of the evaluation tack to signed int32, throwing OverflowException on overflow.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_Ovf_I4">OpCodes.Conv_Ovf_I4</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_ovf_i4
		{
			get { _ilGenerator.Emit(OpCodes.Conv_Ovf_I2_Un); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_Ovf_I4_Un"/>) that
		/// converts the unsigned value on top of the evaluation stack to signed int32, throwing OverflowException on overflow.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_Ovf_I4_Un">OpCodes.Conv_Ovf_I4_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_ovf_i4_un
		{
			get { _ilGenerator.Emit(OpCodes.Conv_Ovf_I4_Un); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_Ovf_I8"/>) that
		/// converts the signed value on top of the evaluation stack to signed int64, throwing OverflowException on overflow.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_Ovf_I8">OpCodes.Conv_Ovf_I8</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_ovf_i8
		{
			get { _ilGenerator.Emit(OpCodes.Conv_Ovf_I8); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_Ovf_I8_Un"/>) that
		/// converts the unsigned value on top of the evaluation stack to signed int64, throwing OverflowException on overflow.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_Ovf_I8_Un">OpCodes.Conv_Ovf_I8_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_ovf_i8_un
		{
			get { _ilGenerator.Emit(OpCodes.Conv_Ovf_I8_Un); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_Ovf_I_Un"/>) that
		/// converts the unsigned value on top of the evaluation stack to signed natural int,
		/// throwing OverflowException on overflow.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_Ovf_I_Un">OpCodes.Conv_Ovf_I_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_ovf_i_un
		{
			get { _ilGenerator.Emit(OpCodes.Conv_Ovf_I_Un); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_Ovf_U"/>) that
		/// converts the signed value on top of the evaluation stack to unsigned natural int,
		/// throwing OverflowException on overflow.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_Ovf_U">OpCodes.Conv_Ovf_U</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_ovf_u
		{
			get { _ilGenerator.Emit(OpCodes.Conv_Ovf_U); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_Ovf_U1"/>) that
		/// converts the signed value on top of the evaluation stack to unsigned int8 and extends it to int32,
		/// throwing OverflowException on overflow.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_Ovf_U1">OpCodes.Conv_Ovf_U1</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_ovf_u1
		{
			get { _ilGenerator.Emit(OpCodes.Conv_Ovf_U1); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_Ovf_U1_Un"/>) that
		/// converts the unsigned value on top of the evaluation stack to unsigned int8 and extends it to int32,
		/// throwing OverflowException on overflow.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_Ovf_U1_Un">OpCodes.Conv_Ovf_U1_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_ovf_u1_un
		{
			get { _ilGenerator.Emit(OpCodes.Conv_Ovf_U1_Un); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_Ovf_U2"/>) that
		/// converts the signed value on top of the evaluation stack to unsigned int16 and extends it to int32,
		/// throwing OverflowException on overflow.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_Ovf_U2">OpCodes.Conv_Ovf_U2</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_ovf_u2
		{
			get { _ilGenerator.Emit(OpCodes.Conv_Ovf_U2); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_Ovf_U2_Un"/>) that
		/// converts the unsigned value on top of the evaluation stack to unsigned int16 and extends it to int32,
		/// throwing OverflowException on overflow.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_Ovf_U2_Un">OpCodes.Conv_Ovf_U2_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_ovf_u2_un
		{
			get { _ilGenerator.Emit(OpCodes.Conv_Ovf_U2_Un); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_Ovf_U4"/>) that
		/// Converts the signed value on top of the evaluation stack to unsigned int32, throwing OverflowException on overflow.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_Ovf_U4">OpCodes.Conv_Ovf_U4</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_ovf_u4
		{
			get { _ilGenerator.Emit(OpCodes.Conv_Ovf_U4); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_Ovf_U4_Un"/>) that
		/// converts the unsigned value on top of the evaluation stack to unsigned int32, throwing OverflowException on overflow.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_Ovf_U4_Un">OpCodes.Conv_Ovf_U4_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_ovf_u4_un
		{
			get { _ilGenerator.Emit(OpCodes.Conv_Ovf_U4_Un); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_Ovf_U8"/>) that
		/// converts the signed value on top of the evaluation stack to unsigned int64, throwing OverflowException on overflow.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_Ovf_U8">OpCodes.Conv_Ovf_U8</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_ovf_u8
		{
			get { _ilGenerator.Emit(OpCodes.Conv_Ovf_U8); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_Ovf_U8_Un"/>) that
		/// converts the unsigned value on top of the evaluation stack to unsigned int64, throwing OverflowException on overflow.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_Ovf_U8_Un">OpCodes.Conv_Ovf_U8_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_ovf_u8_un
		{
			get { _ilGenerator.Emit(OpCodes.Conv_Ovf_U8_Un); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_Ovf_U_Un"/>) that
		/// converts the unsigned value on top of the evaluation stack to unsigned natural int,
		/// throwing OverflowException on overflow.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_Ovf_U_Un">OpCodes.Conv_Ovf_U_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_ovf_u_un
		{
			get { _ilGenerator.Emit(OpCodes.Conv_Ovf_U_Un); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_R4"/>) that
		/// converts the value on top of the evaluation stack to float32.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_R4">OpCodes.Conv_R4</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_r4
		{
			get { _ilGenerator.Emit(OpCodes.Conv_R4); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_R8"/>) that
		/// converts the value on top of the evaluation stack to float64.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_R8">OpCodes.Conv_R8</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_r8
		{
			get { _ilGenerator.Emit(OpCodes.Conv_R8); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_R_Un"/>) that
		/// converts the unsigned integer value on top of the evaluation stack to float32.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_R_Un">OpCodes.Conv_R_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_r_un
		{
			get { _ilGenerator.Emit(OpCodes.Conv_R_Un); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_U"/>) that
		/// converts the value on top of the evaluation stack to unsigned natural int, and extends it to natural int.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_U">OpCodes.Conv_U</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_u
		{
			get { _ilGenerator.Emit(OpCodes.Conv_U); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_U1"/>) that
		/// converts the value on top of the evaluation stack to unsigned int8, and extends it to int32.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_U1">OpCodes.Conv_U1</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_u1
		{
			get { _ilGenerator.Emit(OpCodes.Conv_U1); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_U2"/>) that
		/// converts the value on top of the evaluation stack to unsigned int16, and extends it to int32.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_U2">OpCodes.Conv_U2</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_u2
		{
			get { _ilGenerator.Emit(OpCodes.Conv_U2); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_U4"/>) that
		/// converts the value on top of the evaluation stack to unsigned int32, and extends it to int32.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_U4">OpCodes.Conv_U4</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_u4
		{
			get { _ilGenerator.Emit(OpCodes.Conv_U4); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Conv_U8"/>) that
		/// converts the value on top of the evaluation stack to unsigned int64, and extends it to int64.
		/// </summary>
		/// <seealso cref="OpCodes.Conv_U8">OpCodes.Conv_U8</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper conv_u8
		{
			get { _ilGenerator.Emit(OpCodes.Conv_U8); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Cpblk"/>) that
		/// copies a specified number bytes from a source address to a destination address.
		/// </summary>
		/// <seealso cref="OpCodes.Cpblk">OpCodes.Cpblk</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper cpblk
		{
			get { _ilGenerator.Emit(OpCodes.Cpblk); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Cpobj"/>, type) that
		/// copies the value type located at the address of an object (type &amp;, * or natural int) 
		/// to the address of the destination object (type &amp;, * or natural int).
		/// </summary>
		/// <param name="type">A Type</param>
		/// <seealso cref="OpCodes.Cpobj">OpCodes.Cpobj</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Type)">ILGenerator.Emit</seealso>
		public EmitHelper cpobj(Type type)
		{
			_ilGenerator.Emit(OpCodes.Cpobj, type); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Div"/>) that
		/// divides two values and pushes the result as a floating-point (type F) or
		/// quotient (type int32) onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Div">OpCodes.Div</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper div
		{
			get { _ilGenerator.Emit(OpCodes.Div); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Div_Un"/>) that
		/// divides two unsigned integer values and pushes the result (int32) onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Div_Un">OpCodes.Div_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper div_un
		{
			get { _ilGenerator.Emit(OpCodes.Div_Un); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Dup"/>) that
		/// copies the current topmost value on the evaluation stack, and then pushes the copy onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Dup">OpCodes.Dup</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper dup
		{
			get { _ilGenerator.Emit(OpCodes.Dup); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Endfilter"/>) that
		/// transfers control from the filter clause of an exception back to
		/// the Common Language Infrastructure (CLI) exception handler.
		/// </summary>
		/// <seealso cref="OpCodes.Endfilter">OpCodes.Endfilter</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper endfilter
		{
			get { _ilGenerator.Emit(OpCodes.Endfilter); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Endfinally"/>) that
		/// transfers control from the fault or finally clause of an exception block back to
		/// the Common Language Infrastructure (CLI) exception handler.
		/// </summary>
		/// <seealso cref="OpCodes.Endfinally">OpCodes.Endfinally</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper endfinally
		{
			get { _ilGenerator.Emit(OpCodes.Endfinally); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Initblk"/>) that
		/// initializes a specified block of memory at a specific address to a given size and initial value.
		/// </summary>
		/// <seealso cref="OpCodes.Initblk">OpCodes.Initblk</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper initblk
		{
			get { _ilGenerator.Emit(OpCodes.Initblk); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Initobj"/>, type) that
		/// initializes all the fields of the object at a specific address to a null reference or 
		/// a 0 of the appropriate primitive type.
		/// </summary>
		/// <param name="type">A Type</param>
		/// <seealso cref="OpCodes.Initobj">OpCodes.Initobj</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Type)">ILGenerator.Emit</seealso>
		public EmitHelper initobj(Type type)
		{
			_ilGenerator.Emit(OpCodes.Initobj, type); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Isinst"/>, type) that
		/// tests whether an object reference (type O) is an instance of a particular class.
		/// </summary>
		/// <param name="type">A Type</param>
		/// <seealso cref="OpCodes.Isinst">OpCodes.Isinst</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Type)">ILGenerator.Emit</seealso>
		public EmitHelper isinst(Type type)
		{
			_ilGenerator.Emit(OpCodes.Isinst, type); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Jmp"/>, methodInfo) that
		/// exits current method and jumps to specified method.
		/// </summary>
		/// <param name="methodInfo">The method to be jumped.</param>
		/// <seealso cref="OpCodes.Jmp">OpCodes.Jmp</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,MethodInfo)">ILGenerator.Emit</seealso>
		public EmitHelper jmp(MethodInfo methodInfo)
		{
			_ilGenerator.Emit(OpCodes.Jmp, methodInfo); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldarg"/>, short) that
		/// loads an argument (referenced by a specified index value) onto the stack.
		/// </summary>
		/// <param name="index">Index of the argument that is pushed onto the stack.</param>
		/// <seealso cref="OpCodes.Ldarg">OpCodes.Ldarg</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,short)">ILGenerator.Emit</seealso>
		public EmitHelper ldarg(short index)
		{
			_ilGenerator.Emit(OpCodes.Ldarg, index); return this;
		}

        /*
		public EmitHelper ldargEx(ParameterInfo parameterInfo, bool box)
		{
			ldarg(parameterInfo);

			Type type = parameterInfo.ParameterType;

			if (parameterInfo.ParameterType.IsByRef)
				type = parameterInfo.ParameterType.GetElementType();

			if (parameterInfo.ParameterType.IsByRef)
			{
				if (type.IsValueType && type.IsPrimitive == false)
					ldobj(type);
				else
					ldind(type);
			}

			if (box)
				boxIfValueType(type);

			return this;
		}
        */
		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldarg"/>, short) or 
		/// ILGenerator.Emit(<see cref="OpCodes.Ldarg_S"/>, byte) that
		/// loads an argument (referenced by a specified index value) onto the stack.
		/// </summary>
		/// <param name="index">Index of the argument that is pushed onto the stack.</param>
		/// <seealso cref="OpCodes.Ldarg">OpCodes.Ldarg</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,short)">ILGenerator.Emit</seealso>
		public EmitHelper ldarg(int index)
		{
			switch (index)
			{
				case 0: ldarg_0.end(); break;
				case 1: ldarg_1.end(); break;
				case 2: ldarg_2.end(); break;
				case 3: ldarg_3.end(); break;
				default:
					if      (index <= byte. MaxValue) ldarg_s((byte)index);
					else if (index <= short.MaxValue) ldarg ((short)index);
					else
						throw new ArgumentOutOfRangeException("index");

					break;
			}

			return this;
		}

        /*
		/// <summary>
		/// Loads an argument onto the stack.
		/// </summary>
		/// <param name="parameterInfo">A ParameterInfo representing a parameter.</param>
		public EmitHelper ldarg(ParameterInfo parameterInfo)
		{
			if (parameterInfo == null) throw new ArgumentNullException("parameterInfo");

			bool isStatic = 
				Method is MethodBuilderHelper && ((MethodBuilderHelper)Method).MethodBuilder.IsStatic;

			return ldarg(parameterInfo.Position + (isStatic? 0: 1));
		}
        */

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldarga"/>, short) that
		/// load an argument address onto the evaluation stack.
		/// </summary>
		/// <param name="index">Index of the address addr of the argument that is pushed onto the stack.</param>
		/// <seealso cref="OpCodes.Ldarga">OpCodes.Ldarga</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,short)">ILGenerator.Emit</seealso>
		public EmitHelper ldarga(short index)
		{
			_ilGenerator.Emit(OpCodes.Ldarga, index); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldarga_S"/>, byte) that
		/// load an argument address, in short form, onto the evaluation stack.
		/// </summary>
		/// <param name="index">Index of the address addr of the argument that is pushed onto the stack.</param>
		/// <seealso cref="OpCodes.Ldarga_S">OpCodes.Ldarga_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,byte)">ILGenerator.Emit</seealso>
		public EmitHelper ldarga_s(byte index)
		{
			_ilGenerator.Emit(OpCodes.Ldarga_S, index); return this;
		}

		/// <summary>
		/// Load an argument address onto the evaluation stack.
		/// </summary>
		/// <param name="index">Index of the address addr of the argument that is pushed onto the stack.</param>
		public EmitHelper ldarga(int index)
		{
			if      (index <= byte. MaxValue) ldarga_s((byte)index);
			else if (index <= short.MaxValue) ldarga ((short)index);
			else
				throw new ArgumentOutOfRangeException("index");

			return this;
		}

        /*
		/// <summary>
		/// Loads an argument address onto the stack.
		/// </summary>
		/// <param name="parameterInfo">A ParameterInfo representing a parameter.</param>
		public EmitHelper ldarga(ParameterInfo parameterInfo)
		{
			if (parameterInfo == null) throw new ArgumentNullException("parameterInfo");

			bool isStatic = 
				Method is MethodBuilderHelper && ((MethodBuilderHelper)Method).MethodBuilder.IsStatic;

			return ldarga(parameterInfo.Position + (isStatic? 0: 1));
		}
        */

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldarg_0"/>) that
		/// loads the argument at index 0 onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Ldarg_0">OpCodes.Ldarg_0</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldarg_0
		{
			get { _ilGenerator.Emit(OpCodes.Ldarg_0); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldarg_1"/>) that
		/// loads the argument at index 1 onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Ldarg_1">OpCodes.Ldarg_1</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldarg_1
		{
			get { _ilGenerator.Emit(OpCodes.Ldarg_1); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldarg_2"/>) that
		/// loads the argument at index 2 onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Ldarg_2">OpCodes.Ldarg_2</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldarg_2
		{
			get { _ilGenerator.Emit(OpCodes.Ldarg_2); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldarg_3"/>) that
		/// loads the argument at index 3 onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Ldarg_3">OpCodes.Ldarg_3</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldarg_3
		{
			get { _ilGenerator.Emit(OpCodes.Ldarg_3); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldarg_S"/>, byte) that
		/// loads the argument (referenced by a specified short form index) onto the evaluation stack.
		/// </summary>
		/// <param name="index">Index of the argument value that is pushed onto the stack.</param>
		/// <seealso cref="OpCodes.Ldarg_S">OpCodes.Ldarg_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,byte)">ILGenerator.Emit</seealso>
		public EmitHelper ldarg_s(byte index)
		{
			_ilGenerator.Emit(OpCodes.Ldarg_S, index); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldc_I4"/>, int) that
		/// pushes a supplied value of type int32 onto the evaluation stack as an int32.
		/// </summary>
		/// <param name="num">The value pushed onto the stack.</param>
		/// <seealso cref="OpCodes.Ldc_I4">OpCodes.Ldc_I4</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,int)">ILGenerator.Emit</seealso>
		public EmitHelper ldc_i4(int num)
		{
			_ilGenerator.Emit(OpCodes.Ldc_I4, num); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldc_I4_0"/>) that
		/// pushes the integer value of 0 onto the evaluation stack as an int32.
		/// </summary>
		/// <seealso cref="OpCodes.Ldc_I4_0">OpCodes.Ldc_I4_0</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldc_i4_0
		{
			get { _ilGenerator.Emit(OpCodes.Ldc_I4_0); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldc_I4_1"/>) that
		/// pushes the integer value of 1 onto the evaluation stack as an int32.
		/// </summary>
		/// <seealso cref="OpCodes.Ldc_I4_1">OpCodes.Ldc_I4_1</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldc_i4_1
		{
			get { _ilGenerator.Emit(OpCodes.Ldc_I4_1); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldc_I4_2"/>) that
		/// pushes the integer value of 2 onto the evaluation stack as an int32.
		/// </summary>
		/// <seealso cref="OpCodes.Ldc_I4_2">OpCodes.Ldc_I4_2</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldc_i4_2
		{
			get { _ilGenerator.Emit(OpCodes.Ldc_I4_2); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldc_I4_3"/>) that
		/// pushes the integer value of 3 onto the evaluation stack as an int32.
		/// </summary>
		/// <seealso cref="OpCodes.Ldc_I4_3">OpCodes.Ldc_I4_3</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldc_i4_3
		{
			get { _ilGenerator.Emit(OpCodes.Ldc_I4_3); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldc_I4_4"/>) that
		/// pushes the integer value of 4 onto the evaluation stack as an int32.
		/// </summary>
		/// <seealso cref="OpCodes.Ldc_I4_4">OpCodes.Ldc_I4_4</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldc_i4_4
		{
			get { _ilGenerator.Emit(OpCodes.Ldc_I4_4); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldc_I4_5"/>) that
		/// pushes the integer value of 5 onto the evaluation stack as an int32.
		/// </summary>
		/// <seealso cref="OpCodes.Ldc_I4_5">OpCodes.Ldc_I4_0</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldc_i4_5
		{
			get { _ilGenerator.Emit(OpCodes.Ldc_I4_5); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldc_I4_6"/>) that
		/// pushes the integer value of 6 onto the evaluation stack as an int32.
		/// </summary>
		/// <seealso cref="OpCodes.Ldc_I4_6">OpCodes.Ldc_I4_6</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldc_i4_6
		{
			get { _ilGenerator.Emit(OpCodes.Ldc_I4_6); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldc_I4_7"/>) that
		/// pushes the integer value of 7 onto the evaluation stack as an int32.
		/// </summary>
		/// <seealso cref="OpCodes.Ldc_I4_7">OpCodes.Ldc_I4_7</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldc_i4_7
		{
			get { _ilGenerator.Emit(OpCodes.Ldc_I4_7); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldc_I4_8"/>) that
		/// pushes the integer value of 8 onto the evaluation stack as an int32.
		/// </summary>
		/// <seealso cref="OpCodes.Ldc_I4_8">OpCodes.Ldc_I4_8</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldc_i4_8
		{
			get { _ilGenerator.Emit(OpCodes.Ldc_I4_8); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldc_I4_M1"/>) that
		/// pushes the integer value of -1 onto the evaluation stack as an int32.
		/// </summary>
		/// <seealso cref="OpCodes.Ldc_I4_M1">OpCodes.Ldc_I4_M1</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldc_i4_m1
		{
			get { _ilGenerator.Emit(OpCodes.Ldc_I4_M1); return this; }
		}

		public EmitHelper ldc_i4_(int num)
		{
			switch (num)
			{
				case -1: ldc_i4_m1.end(); break;
				case  0: ldc_i4_0. end(); break;
				case  1: ldc_i4_1. end(); break;
				case  2: ldc_i4_2. end(); break;
				case  3: ldc_i4_3. end(); break;
				case  4: ldc_i4_4. end(); break;
				case  5: ldc_i4_5. end(); break;
				case  6: ldc_i4_6. end(); break;
				case  7: ldc_i4_7. end(); break;
				case  8: ldc_i4_8. end(); break;
				default:
					if (num >= byte.MinValue && num <= byte.MaxValue)
						ldc_i4_s((byte)num);
					else
						ldc_i4   (num);

					break;
			}

			return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldc_I4_S"/>, byte) that
		/// pushes the supplied int8 value onto the evaluation stack as an int32, short form.
		/// </summary>
		/// <param name="num">The value pushed onto the stack.</param>
		/// <seealso cref="OpCodes.Ldc_I4_S">OpCodes.Ldc_I4_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,byte)">ILGenerator.Emit</seealso>
		public EmitHelper ldc_i4_s(byte num)
		{
			_ilGenerator.Emit(OpCodes.Ldc_I4_S, num); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldc_I8"/>, long) that
		/// pushes a supplied value of type int64 onto the evaluation stack as an int64.
		/// </summary>
		/// <param name="num">The value pushed onto the stack.</param>
		/// <seealso cref="OpCodes.Ldc_I8">OpCodes.Ldc_I8</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,long)">ILGenerator.Emit</seealso>
		public EmitHelper ldc_i8(long num)
		{
			_ilGenerator.Emit(OpCodes.Ldc_I8, num); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldc_R4"/>, float) that
		/// pushes a supplied value of type float32 onto the evaluation stack as type F (float).
		/// </summary>
		/// <param name="num">The value pushed onto the stack.</param>
		/// <seealso cref="OpCodes.Ldc_R4">OpCodes.Ldc_R4</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,float)">ILGenerator.Emit</seealso>
		public EmitHelper ldc_r4(float num)
		{
			_ilGenerator.Emit(OpCodes.Ldc_R4, num); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldc_R8"/>, double) that
		/// pushes a supplied value of type float64 onto the evaluation stack as type F (float).
		/// </summary>
		/// <param name="num">The value pushed onto the stack.</param>
		/// <seealso cref="OpCodes.Ldc_R8">OpCodes.Ldc_R8</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,double)">ILGenerator.Emit</seealso>
		public EmitHelper ldc_r8(double num)
		{
			_ilGenerator.Emit(OpCodes.Ldc_R8, num); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldelema"/>, type) that
		/// loads the address of the array element at a specified array index onto the top of the evaluation stack 
		/// as type &amp; (managed pointer).
		/// </summary>
		/// <param name="type">A Type</param>
		/// <seealso cref="OpCodes.Ldelema">OpCodes.Ldelema</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Type)">ILGenerator.Emit</seealso>
		public EmitHelper ldelema(Type type)
		{
			_ilGenerator.Emit(OpCodes.Ldelema, type); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldelem_I"/>) that
		/// loads the element with type natural int at a specified array index onto the top of the evaluation stack 
		/// as a natural int.
		/// </summary>
		/// <seealso cref="OpCodes.Ldelem_I">OpCodes.Ldelem_I</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldelem_i
		{
			get { _ilGenerator.Emit(OpCodes.Ldelem_I); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldelem_I1"/>) that
		/// loads the element with type int8 at a specified array index onto the top of the evaluation stack as an int32.
		/// </summary>
		/// <seealso cref="OpCodes.Ldelem_I1">OpCodes.Ldelem_I1</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldelem_i1
		{
			get { _ilGenerator.Emit(OpCodes.Ldelem_I1); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldelem_I2"/>) that
		/// loads the element with type int16 at a specified array index onto the top of the evaluation stack as an int32.
		/// </summary>
		/// <seealso cref="OpCodes.Ldelem_I2">OpCodes.Ldelem_I2</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldelem_i2
		{
			get { _ilGenerator.Emit(OpCodes.Ldelem_I2); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldelem_I4"/>) that
		/// loads the element with type int32 at a specified array index onto the top of the evaluation stack as an int32.
		/// </summary>
		/// <seealso cref="OpCodes.Ldelem_I4">OpCodes.Ldelem_I4</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldelem_i4
		{
			get { _ilGenerator.Emit(OpCodes.Ldelem_I4); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldelem_I8"/>) that
		/// loads the element with type int64 at a specified array index onto the top of the evaluation stack as an int64.
		/// </summary>
		/// <seealso cref="OpCodes.Ldelem_I8">OpCodes.Ldelem_I8</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldelem_i8
		{
			get { _ilGenerator.Emit(OpCodes.Ldelem_I8); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldelem_R4"/>) that
		/// loads the element with type float32 at a specified array index onto the top of the evaluation stack as type F (float).
		/// </summary>
		/// <seealso cref="OpCodes.Ldelem_R4">OpCodes.Ldelem_R4</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldelem_r4
		{
			get { _ilGenerator.Emit(OpCodes.Ldelem_R4); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldelem_R8"/>) that
		/// loads the element with type float64 at a specified array index onto the top of the evaluation stack as type F (float).
		/// </summary>
		/// <seealso cref="OpCodes.Ldelem_R8">OpCodes.Ldelem_R8</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldelem_r8
		{
			get { _ilGenerator.Emit(OpCodes.Ldelem_R8); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldelem_Ref"/>) that
		/// loads the element containing an object reference at a specified array index 
		/// onto the top of the evaluation stack as type O (object reference).
		/// </summary>
		/// <seealso cref="OpCodes.Ldelem_Ref">OpCodes.Ldelem_Ref</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldelem_ref
		{
			get { _ilGenerator.Emit(OpCodes.Ldelem_Ref); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldelem_U1"/>) that
		/// loads the element with type unsigned int8 at a specified array index onto the top of the evaluation stack as an int32.
		/// </summary>
		/// <seealso cref="OpCodes.Ldelem_U1">OpCodes.Ldelem_U1</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldelem_u1
		{
			get { _ilGenerator.Emit(OpCodes.Ldelem_U1); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldelem_U2"/>) that
		/// loads the element with type unsigned int16 at a specified array index 
		/// onto the top of the evaluation stack as an int32.
		/// </summary>
		/// <seealso cref="OpCodes.Ldelem_U2">OpCodes.Ldelem_U2</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldelem_u2
		{
			get { _ilGenerator.Emit(OpCodes.Ldelem_U2); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldelem_U4"/>) that
		/// loads the element with type unsigned int32 at a specified array index 
		/// onto the top of the evaluation stack as an int32.
		/// </summary>
		/// <seealso cref="OpCodes.Ldelem_U4">OpCodes.Ldelem_U4</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldelem_u4
		{
			get { _ilGenerator.Emit(OpCodes.Ldelem_U4); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldfld"/>, fieldInfo) that
		/// finds the value of a field in the object whose reference is currently on the evaluation stack.
		/// </summary>
		/// <param name="fieldInfo">A FieldInfo representing a field.</param>
		/// <seealso cref="OpCodes.Ldfld">OpCodes.Ldfld</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,FieldInfo)">ILGenerator.Emit</seealso>
		public EmitHelper ldfld(FieldInfo fieldInfo)
		{
			_ilGenerator.Emit(OpCodes.Ldfld, fieldInfo); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldflda"/>, fieldInfo) that
		/// finds the address of a field in the object whose reference is currently on the evaluation stack.
		/// </summary>
		/// <param name="fieldInfo">A FieldInfo representing a field.</param>
		/// <seealso cref="OpCodes.Ldflda">OpCodes.Ldflda</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,FieldInfo)">ILGenerator.Emit</seealso>
		public EmitHelper ldflda(FieldInfo fieldInfo)
		{
			_ilGenerator.Emit(OpCodes.Ldflda, fieldInfo); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldftn"/>, methodInfo) that
		/// pushes an unmanaged pointer (type natural int) to the native code implementing a specific method 
		/// onto the evaluation stack.
		/// </summary>
		/// <param name="methodInfo">The method to be called.</param>
		/// <seealso cref="OpCodes.Ldftn">OpCodes.Ldftn</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,MethodInfo)">ILGenerator.Emit</seealso>
		public EmitHelper ldftn(MethodInfo methodInfo)
		{
			_ilGenerator.Emit(OpCodes.Ldftn, methodInfo); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldind_I"/>) that
		/// loads a value of type natural int as a natural int onto the evaluation stack indirectly.
		/// </summary>
		/// <seealso cref="OpCodes.Ldind_I">OpCodes.Ldind_I</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldind_i
		{
			get { _ilGenerator.Emit(OpCodes.Ldind_I); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldind_I1"/>) that
		/// loads a value of type int8 as an int32 onto the evaluation stack indirectly.
		/// </summary>
		/// <seealso cref="OpCodes.Ldind_I1">OpCodes.Ldind_I1</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldind_i1
		{
			get { _ilGenerator.Emit(OpCodes.Ldind_I1); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldind_I2"/>) that
		/// loads a value of type int16 as an int32 onto the evaluation stack indirectly.
		/// </summary>
		/// <seealso cref="OpCodes.Ldind_I2">OpCodes.Ldind_I2</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldind_i2
		{
			get { _ilGenerator.Emit(OpCodes.Ldind_I2); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldind_I4"/>) that
		/// loads a value of type int32 as an int32 onto the evaluation stack indirectly.
		/// </summary>
		/// <seealso cref="OpCodes.Ldind_I4">OpCodes.Ldind_I4</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldind_i4
		{
			get { _ilGenerator.Emit(OpCodes.Ldind_I4); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldind_I8"/>) that
		/// loads a value of type int64 as an int64 onto the evaluation stack indirectly.
		/// </summary>
		/// <seealso cref="OpCodes.Ldind_I8">OpCodes.Ldind_I8</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldind_i8
		{
			get { _ilGenerator.Emit(OpCodes.Ldind_I8); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldind_R4"/>) that
		/// loads a value of type float32 as a type F (float) onto the evaluation stack indirectly.
		/// </summary>
		/// <seealso cref="OpCodes.Ldind_R4">OpCodes.Ldind_R4</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldind_r4
		{
			get { _ilGenerator.Emit(OpCodes.Ldind_R4); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldind_R8"/>) that
		/// loads a value of type float64 as a type F (float) onto the evaluation stack indirectly.
		/// </summary>
		/// <seealso cref="OpCodes.Ldind_R8">OpCodes.Ldind_R8</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldind_r8
		{
			get { _ilGenerator.Emit(OpCodes.Ldind_R8); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldind_Ref"/>) that
		/// loads an object reference as a type O (object reference) onto the evaluation stack indirectly.
		/// </summary>
		/// <seealso cref="OpCodes.Ldind_Ref">OpCodes.Ldind_Ref</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldind_ref
		{
			get { _ilGenerator.Emit(OpCodes.Ldind_Ref); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldind_U1"/>) that
		/// loads a value of type unsigned int8 as an int32 onto the evaluation stack indirectly.
		/// </summary>
		/// <seealso cref="OpCodes.Ldind_U1">OpCodes.Ldind_U1</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldind_u1
		{
			get { _ilGenerator.Emit(OpCodes.Ldind_U1); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldind_U2"/>) that
		/// loads a value of type unsigned int16 as an int32 onto the evaluation stack indirectly.
		/// </summary>
		/// <seealso cref="OpCodes.Ldind_U2">OpCodes.Ldind_U2</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldind_u2
		{
			get { _ilGenerator.Emit(OpCodes.Ldind_U2); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldind_U4"/>) that
		/// loads a value of type unsigned int32 as an int32 onto the evaluation stack indirectly.
		/// </summary>
		/// <seealso cref="OpCodes.Ldind_U4">OpCodes.Ldind_U4</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldind_u4
		{
			get { _ilGenerator.Emit(OpCodes.Ldind_U4); return this; }
		}

		/// <summary>
		/// Loads a value of the type from a supplied address.
		/// </summary>
		/// <param name="type">A Type.</param>
		public EmitHelper ldind(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			if      (type.IsClass)           ldind_ref.end();
			else if (type == typeof(int))    ldind_i4.end();
			else if (type == typeof(uint))   ldind_u4.end();
			else if (type == typeof(bool)  ||
			         type == typeof(byte))   ldind_u1.end();
			else if (type == typeof(sbyte))  ldind_i1.end();
			else if (type == typeof(char)  ||
			         type == typeof(ushort)) ldind_u2.end();
			else if (type == typeof(short))  ldind_i2.end();
			else if (type == typeof(double)) ldind_r8.end();
			else if (type == typeof(float))  ldind_r4.end();
			else if (type == typeof(long)  ||
			         type == typeof(ulong))  ldind_i8.end();
			else
				throw new InvalidOperationException();

			return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldlen"/>) that
		/// pushes the number of elements of a zero-based, one-dimensional array onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Ldlen">OpCodes.Ldlen</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldlen
		{
			get { _ilGenerator.Emit(OpCodes.Ldlen); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldloc"/>, short) that
		/// load an argument address onto the evaluation stack.
		/// </summary>
		/// <param name="index">Index of the local variable value pushed onto the stack.</param>
		/// <seealso cref="OpCodes.Ldloc">OpCodes.Ldloc</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,short)">ILGenerator.Emit</seealso>
		public EmitHelper ldloc(short index)
		{
			_ilGenerator.Emit(OpCodes.Ldloc, index); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldloc"/>, LocalBuilder) that
		/// load an argument address onto the evaluation stack.
		/// </summary>
		/// <param name="localBuilder">Local variable builder.</param>
		/// <seealso cref="OpCodes.Ldloc">OpCodes.Ldloc</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,short)">ILGenerator.Emit</seealso>
		public EmitHelper ldloc(LocalBuilder localBuilder)
		{
			_ilGenerator.Emit(OpCodes.Ldloc, localBuilder); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldloca"/>, short) that
		/// loads the address of the local variable at a specific index onto the evaluation stack.
		/// </summary>
		/// <param name="index">Index of the local variable.</param>
		/// <seealso cref="OpCodes.Ldloca">OpCodes.Ldloca</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,short)">ILGenerator.Emit</seealso>
		public EmitHelper ldloca(short index)
		{
			_ilGenerator.Emit(OpCodes.Ldloca, index); return this;
		}
		
		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldloca_S"/>, byte) that
		/// loads the address of the local variable at a specific index onto the evaluation stack, short form.
		/// </summary>
		/// <param name="index">Index of the local variable.</param>
		/// <seealso cref="OpCodes.Ldloca_S">OpCodes.Ldloca_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,byte)">ILGenerator.Emit</seealso>
		public EmitHelper ldloca_s(byte index)
		{
			_ilGenerator.Emit(OpCodes.Ldloca_S, index); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldloca"/>, LocalBuilder) that
		/// loads the address of the local variable at a specific index onto the evaluation stack.
		/// </summary>
		/// <param name="local">A <see cref="LocalBuilder"/> representing the local variable.</param>
		/// <seealso cref="OpCodes.Ldloca">OpCodes.Ldloca</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,short)">ILGenerator.Emit</seealso>
		public EmitHelper ldloca(LocalBuilder local)
		{
			_ilGenerator.Emit(OpCodes.Ldloca, local); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldloc_0"/>) that
		/// loads the local variable at index 0 onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Ldloc_0">OpCodes.Ldloc_0</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldloc_0
		{
			get { _ilGenerator.Emit(OpCodes.Ldloc_0); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldloc_1"/>) that
		/// loads the local variable at index 1 onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Ldloc_1">OpCodes.Ldloc_1</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldloc_1
		{
			get { _ilGenerator.Emit(OpCodes.Ldloc_1); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldloc_2"/>) that
		/// loads the local variable at index 2 onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Ldloc_2">OpCodes.Ldloc_2</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldloc_2
		{
			get { _ilGenerator.Emit(OpCodes.Ldloc_2); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldloc_3"/>) that
		/// loads the local variable at index 3 onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Ldloc_3">OpCodes.Ldloc_3</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldloc_3
		{
			get { _ilGenerator.Emit(OpCodes.Ldloc_3); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldloc_S"/>, byte) that
		/// loads the local variable at a specific index onto the evaluation stack, short form.
		/// </summary>
		/// <param name="index">Index of the local variable.</param>
		/// <seealso cref="OpCodes.Ldloc_S">OpCodes.Ldloc_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,byte)">ILGenerator.Emit</seealso>
		public EmitHelper ldloc_s(byte index)
		{
			_ilGenerator.Emit(OpCodes.Ldloca_S, index); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldnull"/>) that
		/// pushes a null reference (type O) onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Ldnull">OpCodes.Ldnull</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ldnull
		{
			get { _ilGenerator.Emit(OpCodes.Ldnull); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldobj"/>, type) that
		/// copies the value type object pointed to by an address to the top of the evaluation stack.
		/// </summary>
		/// <param name="type">A Type</param>
		/// <seealso cref="OpCodes.Ldobj">OpCodes.Ldobj</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Type)">ILGenerator.Emit</seealso>
		public EmitHelper ldobj(Type type)
		{
			_ilGenerator.Emit(OpCodes.Ldobj, type); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldsfld"/>, fieldInfo) that
		/// pushes the value of a static field onto the evaluation stack.
		/// </summary>
		/// <param name="fieldInfo">A FieldInfo representing a field.</param>
		/// <seealso cref="OpCodes.Ldsfld">OpCodes.Ldsfld</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,FieldInfo)">ILGenerator.Emit</seealso>
		public EmitHelper ldsfld(FieldInfo fieldInfo)
		{
			_ilGenerator.Emit(OpCodes.Ldsfld, fieldInfo); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldsflda"/>, fieldInfo) that
		/// pushes the address of a static field onto the evaluation stack.
		/// </summary>
		/// <param name="fieldInfo">A FieldInfo representing a field.</param>
		/// <seealso cref="OpCodes.Ldsflda">OpCodes.Ldsflda</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,FieldInfo)">ILGenerator.Emit</seealso>
		public EmitHelper ldsflda(FieldInfo fieldInfo)
		{
			_ilGenerator.Emit(OpCodes.Ldsflda, fieldInfo); return this;
		}

		public EmitHelper ldstrEx(string str)
		{
			return str == null ? ldnull : ldstr(str);
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldstr"/>, string) that
		/// pushes a new object reference to a string literal stored in the metadata.
		/// </summary>
		/// <param name="str">The String to be emitted.</param>
		/// <seealso cref="OpCodes.Ldstr">OpCodes.Ldstr</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,FieldInfo)">ILGenerator.Emit</seealso>
		public EmitHelper ldstr(string str)
		{
			_ilGenerator.Emit(OpCodes.Ldstr, str); return this;
		}

        /*
		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldstr"/>, string) that
		/// pushes a new object reference to a string literal stored in the metadata.
		/// </summary>
		/// <param name="nameOrIndex">The NameOrIndexParameter to be emitted.</param>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,FieldInfo)">ILGenerator.Emit</seealso>
		public EmitHelper ldNameOrIndex(NameOrIndexParameter nameOrIndex)
		{
			return nameOrIndex.ByName?
				ldstr  (nameOrIndex.Name) .call(typeof(NameOrIndexParameter), "op_Implicit", typeof(string)):
				ldc_i4_(nameOrIndex.Index).call(typeof(NameOrIndexParameter), "op_Implicit", typeof(int));
		}
        */

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldtoken"/>, methodInfo) that
		/// converts a metadata token to its runtime representation, pushing it onto the evaluation stack.
		/// </summary>
		/// <param name="methodInfo">The method to be called.</param>
		/// <seealso cref="OpCodes.Ldtoken">OpCodes.Ldtoken</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,MethodInfo)">ILGenerator.Emit</seealso>
		public EmitHelper ldtoken(MethodInfo methodInfo)
		{
			_ilGenerator.Emit(OpCodes.Ldtoken, methodInfo); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldtoken"/>, fieldInfo) that
		/// converts a metadata token to its runtime representation, 
		/// pushing it onto the evaluation stack.
		/// </summary>
		/// <param name="fieldInfo">A FieldInfo representing a field.</param>
		/// <seealso cref="OpCodes.Ldtoken">OpCodes.Ldtoken</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,FieldInfo)">ILGenerator.Emit</seealso>
		public EmitHelper ldtoken(FieldInfo fieldInfo)
		{
			_ilGenerator.Emit(OpCodes.Ldtoken, fieldInfo); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldtoken"/>, type) that
		/// converts a metadata token to its runtime representation, pushing it onto the evaluation stack.
		/// </summary>
		/// <param name="type">A Type</param>
		/// <seealso cref="OpCodes.Ldtoken">OpCodes.Ldtoken</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Type)">ILGenerator.Emit</seealso>
		public EmitHelper ldtoken(Type type)
		{
			_ilGenerator.Emit(OpCodes.Ldtoken, type); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ldvirtftn"/>, methodInfo) that
		/// pushes an unmanaged pointer (type natural int) to the native code implementing a particular virtual method 
		/// associated with a specified object onto the evaluation stack.
		/// </summary>
		/// <param name="methodInfo">The method to be called.</param>
		/// <seealso cref="OpCodes.Ldvirtftn">OpCodes.Ldvirtftn</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,MethodInfo)">ILGenerator.Emit</seealso>
		public EmitHelper ldvirtftn(MethodInfo methodInfo)
		{
			_ilGenerator.Emit(OpCodes.Ldvirtftn, methodInfo); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Leave"/>, label) that
		/// exits a protected region of code, unconditionally tranferring control to a specific target instruction.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <seealso cref="OpCodes.Leave">OpCodes.Leave</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper leave(Label label)
		{
			_ilGenerator.Emit(OpCodes.Leave, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Leave_S"/>, label) that
		/// exits a protected region of code, unconditionally tranferring control to a target instruction (short form).
		/// </summary>
		/// <param name="label">The label.</param>
		/// <seealso cref="OpCodes.Leave_S">OpCodes.Leave_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper leave_s(Label label)
		{
			_ilGenerator.Emit(OpCodes.Leave_S, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Localloc"/>) that
		/// allocates a certain number of bytes from the local dynamic memory pool and pushes the address 
		/// (a transient pointer, type *) of the first allocated byte onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Localloc">OpCodes.Localloc</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper localloc
		{
			get { _ilGenerator.Emit(OpCodes.Localloc); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Mkrefany"/>, type) that
		/// pushes a typed reference to an instance of a specific type onto the evaluation stack.
		/// </summary>
		/// <param name="type">A Type</param>
		/// <seealso cref="OpCodes.Mkrefany">OpCodes.Mkrefany</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Type)">ILGenerator.Emit</seealso>
		public EmitHelper mkrefany(Type type)
		{
			_ilGenerator.Emit(OpCodes.Mkrefany, type); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Mul"/>) that
		/// multiplies two values and pushes the result on the evaluation stack.
		/// (a transient pointer, type *) of the first allocated byte onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Mul">OpCodes.Mul</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper mul
		{
			get { _ilGenerator.Emit(OpCodes.Mul); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Mul_Ovf"/>) that
		/// multiplies two integer values, performs an overflow check, 
		/// and pushes the result onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Mul_Ovf">OpCodes.Mul_Ovf</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper mul_ovf
		{
			get { _ilGenerator.Emit(OpCodes.Mul_Ovf); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Mul_Ovf_Un"/>) that
		/// multiplies two unsigned integer values, performs an overflow check, 
		/// and pushes the result onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Mul_Ovf_Un">OpCodes.Mul_Ovf_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper mul_ovf_un
		{
			get { _ilGenerator.Emit(OpCodes.Mul_Ovf_Un); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Neg"/>) that
		/// negates a value and pushes the result onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Neg">OpCodes.Neg</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper neg
		{
			get { _ilGenerator.Emit(OpCodes.Neg); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Newarr"/>, type) that
		/// pushes an object reference to a new zero-based, one-dimensional array whose elements 
		/// are of a specific type onto the evaluation stack.
		/// </summary>
		/// <param name="type">A Type</param>
		/// <seealso cref="OpCodes.Newarr">OpCodes.Newarr</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Type)">ILGenerator.Emit</seealso>
		public EmitHelper newarr(Type type)
		{
			_ilGenerator.Emit(OpCodes.Newarr, type); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Newobj"/>, ConstructorInfo) that
		/// creates a new object or a new instance of a value type,
		/// pushing an object reference (type O) onto the evaluation stack.
		/// </summary>
		/// <param name="constructorInfo">A ConstructorInfo representing a constructor.</param>
		/// <seealso cref="OpCodes.Newobj">OpCodes.Newobj</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,ConstructorInfo)">ILGenerator.Emit</seealso>
		public EmitHelper newobj(ConstructorInfo constructorInfo)
		{
			_ilGenerator.Emit(OpCodes.Newobj, constructorInfo); return this;
		}

		public EmitHelper newobj(Type type, params Type[] parameters)
		{
			if (type == null) throw new ArgumentNullException("type");

			ConstructorInfo ci = type.GetConstructor(parameters);

			return newobj(ci);
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Nop"/>) that
		/// fills space if opcodes are patched. No meaningful operation is performed although 
		/// a processing cycle can be consumed.
		/// </summary>
		/// <seealso cref="OpCodes.Nop">OpCodes.Nop</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper nop
		{
			get { _ilGenerator.Emit(OpCodes.Nop); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Not"/>) that
		/// computes the bitwise complement of the integer value on top of the stack 
		/// and pushes the result onto the evaluation stack as the same type.
		/// </summary>
		/// <seealso cref="OpCodes.Not">OpCodes.Not</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper not
		{
			get { _ilGenerator.Emit(OpCodes.Not); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Or"/>) that
		/// compute the bitwise complement of the two integer values on top of the stack and 
		/// pushes the result onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Or">OpCodes.Or</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper or
		{
			get { _ilGenerator.Emit(OpCodes.Or); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Pop"/>) that
		/// removes the value currently on top of the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Pop">OpCodes.Pop</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper pop
		{
			get { _ilGenerator.Emit(OpCodes.Pop); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Refanytype"/>) that
		/// retrieves the type token embedded in a typed reference.
		/// </summary>
		/// <seealso cref="OpCodes.Refanytype">OpCodes.Refanytype</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper refanytype
		{
			get { _ilGenerator.Emit(OpCodes.Refanytype); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Refanyval"/>, type) that
		/// retrieves the address (type &amp;) embedded in a typed reference.
		/// </summary>
		/// <param name="type">A Type</param>
		/// <seealso cref="OpCodes.Refanyval">OpCodes.Refanyval</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Type)">ILGenerator.Emit</seealso>
		public EmitHelper refanyval(Type type)
		{
			_ilGenerator.Emit(OpCodes.Refanyval, type); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Rem"/>) that
		/// divides two values and pushes the remainder onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Rem">OpCodes.Rem</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper rem
		{
			get { _ilGenerator.Emit(OpCodes.Rem); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Rem_Un"/>) that
		/// divides two unsigned values and pushes the remainder onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Rem_Un">OpCodes.Rem_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper rem_un
		{
			get { _ilGenerator.Emit(OpCodes.Rem_Un); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Ret"/>) that
		/// returns from the current method, pushing a return value (if present) 
		/// from the caller's evaluation stack onto the callee's evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Ret">OpCodes.Ret</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper ret()
		{
			_ilGenerator.Emit(OpCodes.Ret); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Rethrow"/>) that
		/// rethrows the current exception.
		/// </summary>
		/// <seealso cref="OpCodes.Rethrow">OpCodes.Rethrow</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper rethrow
		{
			get { _ilGenerator.Emit(OpCodes.Rethrow); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Shl"/>) that
		/// shifts an integer value to the left (in zeroes) by a specified number of bits,
		/// pushing the result onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Shl">OpCodes.Shl</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper shl
		{
			get { _ilGenerator.Emit(OpCodes.Shl); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Shr"/>) that
		/// shifts an integer value (in sign) to the right by a specified number of bits,
		/// pushing the result onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Shr">OpCodes.Shr</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper shr
		{
			get { _ilGenerator.Emit(OpCodes.Shr); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Shr_Un"/>) that
		/// shifts an unsigned integer value (in zeroes) to the right by a specified number of bits,
		/// pushing the result onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Shr_Un">OpCodes.Shr_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper shr_un
		{
			get { _ilGenerator.Emit(OpCodes.Shr_Un); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Sizeof"/>, type) that
		/// pushes the size, in bytes, of a supplied value type onto the evaluation stack.
		/// </summary>
		/// <param name="type">A Type</param>
		/// <seealso cref="OpCodes.Sizeof">OpCodes.Sizeof</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Type)">ILGenerator.Emit</seealso>
		public EmitHelper @sizeof(Type type)
		{
			_ilGenerator.Emit(OpCodes.Sizeof, type); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Starg"/>, short) that
		/// stores the value on top of the evaluation stack in the argument slot at a specified index.
		/// </summary>
		/// <param name="index">Slot index.</param>
		/// <seealso cref="OpCodes.Starg">OpCodes.Starg</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,short)">ILGenerator.Emit</seealso>
		public EmitHelper starg(short index)
		{
			_ilGenerator.Emit(OpCodes.Starg, index); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Starg_S"/>, byte) that
		/// stores the value on top of the evaluation stack in the argument slot at a specified index,
		/// short form.
		/// </summary>
		/// <param name="index">Slot index.</param>
		/// <seealso cref="OpCodes.Starg_S">OpCodes.Starg_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,byte)">ILGenerator.Emit</seealso>
		public EmitHelper starg_s(byte index)
		{
			_ilGenerator.Emit(OpCodes.Starg_S, index); return this;
		}

		/// <summary>
		/// Stores the value on top of the evaluation stack in the argument slot at a specified index.
		/// </summary>
		/// <param name="index">Slot index.</param>
		/// <seealso cref="OpCodes.Starg">OpCodes.Starg</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,short)">ILGenerator.Emit</seealso>
		public EmitHelper starg(int index)
		{
			if      (index < byte. MaxValue) starg_s((byte)index);
			else if (index < short.MaxValue) starg ((short)index);
			else
				throw new ArgumentOutOfRangeException("index");

			return this;
		}

		//	switch (index)
		//	{
		//		case 0: stloc_0.end(); break;
		//		case 1: stloc_1.end(); break;
		//		case 2: stloc_2.end(); break;
		//		case 3: stloc_3.end(); break;
		//		default:
		//			if (index < byte.MinValue) stloc_s((byte)index);
		//			else                       stloc ((short)index);

		//			break;
		//	}

		//	return this;
		//}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stelem_I"/>) that
		/// replaces the array element at a given index with the natural int value 
		/// on the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Stelem_I">OpCodes.Stelem_I</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper stelem_i
		{
			get { _ilGenerator.Emit(OpCodes.Stelem_I); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stelem_I1"/>) that
		/// replaces the array element at a given index with the int8 value on the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Stelem_I1">OpCodes.Stelem_I1</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper stelem_i1
		{
			get { _ilGenerator.Emit(OpCodes.Stelem_I1); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stelem_I2"/>) that
		/// replaces the array element at a given index with the int16 value on the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Stelem_I2">OpCodes.Stelem_I2</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper stelem_i2
		{
			get { _ilGenerator.Emit(OpCodes.Stelem_I2); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stelem_I4"/>) that
		/// replaces the array element at a given index with the int32 value on the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Stelem_I4">OpCodes.Stelem_I4</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper stelem_i4
		{
			get { _ilGenerator.Emit(OpCodes.Stelem_I4); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stelem_I8"/>) that
		/// replaces the array element at a given index with the int64 value on the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Stelem_I8">OpCodes.Stelem_I8</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper stelem_i8
		{
			get { _ilGenerator.Emit(OpCodes.Stelem_I8); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stelem_R4"/>) that
		/// replaces the array element at a given index with the float32 value on the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Stelem_R4">OpCodes.Stelem_R4</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper stelem_r4
		{
			get { _ilGenerator.Emit(OpCodes.Stelem_R4); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stelem_R8"/>) that
		/// replaces the array element at a given index with the float64 value on the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Stelem_R8">OpCodes.Stelem_R8</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper stelem_r8
		{
			get { _ilGenerator.Emit(OpCodes.Stelem_R8); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stelem_Ref"/>) that
		/// replaces the array element at a given index with the object ref value (type O)
		/// on the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Stelem_Ref">OpCodes.Stelem_Ref</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper stelem_ref
		{
			get { _ilGenerator.Emit(OpCodes.Stelem_Ref); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stfld"/>, FieldInfo) that
		/// replaces the value stored in the field of an object reference or pointer with a new value.
		/// </summary>
		/// <param name="fieldInfo">A FieldInfo representing a field.</param>
		/// <seealso cref="OpCodes.Stfld">OpCodes.Stfld</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,FieldInfo)">ILGenerator.Emit</seealso>
		public EmitHelper stfld(FieldInfo fieldInfo)
		{
			_ilGenerator.Emit(OpCodes.Stfld, fieldInfo); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stind_I"/>) that
		/// stores a value of type natural int at a supplied address.
		/// </summary>
		/// <seealso cref="OpCodes.Stind_I">OpCodes.Stind_I</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper stind_i
		{
			get { _ilGenerator.Emit(OpCodes.Stind_I); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stind_I1"/>) that
		/// stores a value of type int8 at a supplied address.
		/// </summary>
		/// <seealso cref="OpCodes.Stind_I1">OpCodes.Stind_I1</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper stind_i1
		{
			get { _ilGenerator.Emit(OpCodes.Stind_I1); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stind_I2"/>) that
		/// stores a value of type int16 at a supplied address.
		/// </summary>
		/// <seealso cref="OpCodes.Stind_I2">OpCodes.Stind_I2</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper stind_i2
		{
			get { _ilGenerator.Emit(OpCodes.Stind_I2); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stind_I4"/>) that
		/// stores a value of type int32 at a supplied address.
		/// </summary>
		/// <seealso cref="OpCodes.Stind_I4">OpCodes.Stind_I4</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper stind_i4
		{
			get { _ilGenerator.Emit(OpCodes.Stind_I4); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stind_I8"/>) that
		/// stores a value of type int64 at a supplied address.
		/// </summary>
		/// <seealso cref="OpCodes.Stind_I8">OpCodes.Stind_I8</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper stind_i8
		{
			get { _ilGenerator.Emit(OpCodes.Stind_I8); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stind_R4"/>) that
		/// stores a value of type float32 at a supplied address.
		/// </summary>
		/// <seealso cref="OpCodes.Stind_R4">OpCodes.Stind_R4</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper stind_r4
		{
			get { _ilGenerator.Emit(OpCodes.Stind_R4); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stind_R8"/>) that
		/// stores a value of type float64 at a supplied address.
		/// </summary>
		/// <seealso cref="OpCodes.Stind_R8">OpCodes.Stind_R8</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper stind_r8
		{
			get { _ilGenerator.Emit(OpCodes.Stind_R8); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stind_Ref"/>) that
		/// stores an object reference value at a supplied address.
		/// </summary>
		/// <seealso cref="OpCodes.Stind_Ref">OpCodes.Stind_Ref</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper stind_ref
		{
			get { _ilGenerator.Emit(OpCodes.Stind_Ref); return this; }
		}

		/// <summary>
		/// Stores a value of the type at a supplied address.
		/// </summary>
		/// <param name="type">A Type.</param>
		public EmitHelper stind(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			if      (type.IsClass)           stind_ref.end();
			else if (type == typeof(int)   ||
			         type == typeof(uint))   stind_i4.end();
			else if (type == typeof(bool)  ||
			         type == typeof(byte)  ||
			         type == typeof(sbyte))  stind_i1.end();
			else if (type == typeof(char)  ||
			         type == typeof(short) ||
			         type == typeof(ushort)) stind_i2.end();
			else if (type == typeof(double)) stind_r8.end();
			else if (type == typeof(float))  stind_r4.end();
			else if (type == typeof(long)  ||
			         type == typeof(ulong))  stind_i8.end();
			else if (type.IsValueType)       stobj(type);
			else
				throw new InvalidOperationException();

			return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stloc"/>, LocalBuilder) that
		/// pops the current value from the top of the evaluation stack and stores it 
		/// in the local variable list at a specified index.
		/// </summary>
		/// <param name="local">A local variable.</param>
		/// <seealso cref="OpCodes.Stloc">OpCodes.Stloc</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,LocalBuilder)">ILGenerator.Emit</seealso>
		public EmitHelper stloc(LocalBuilder local)
		{
			_ilGenerator.Emit(OpCodes.Stloc, local); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stloc"/>, short) that
		/// pops the current value from the top of the evaluation stack and stores it 
		/// in the local variable list at a specified index.
		/// </summary>
		/// <param name="index">A local variable index.</param>
		/// <seealso cref="OpCodes.Stloc">OpCodes.Stloc</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,short)">ILGenerator.Emit</seealso>
		public EmitHelper stloc(short index)
		{
			_ilGenerator.Emit(OpCodes.Stloc, index); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stloc_0"/>) that
		/// pops the current value from the top of the evaluation stack and stores it 
		/// in the local variable list at index 0.
		/// </summary>
		/// <seealso cref="OpCodes.Stloc_0">OpCodes.Stloc_0</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper stloc_0
		{
			get { _ilGenerator.Emit(OpCodes.Stloc_0); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stloc_1"/>) that
		/// pops the current value from the top of the evaluation stack and stores it 
		/// in the local variable list at index 1.
		/// </summary>
		/// <seealso cref="OpCodes.Stloc_1">OpCodes.Stloc_1</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper stloc_1
		{
			get { _ilGenerator.Emit(OpCodes.Stloc_1); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stloc_2"/>) that
		/// pops the current value from the top of the evaluation stack and stores it 
		/// in the local variable list at index 2.
		/// </summary>
		/// <seealso cref="OpCodes.Stloc_2">OpCodes.Stloc_2</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper stloc_2
		{
			get { _ilGenerator.Emit(OpCodes.Stloc_2); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stloc_3"/>) that
		/// pops the current value from the top of the evaluation stack and stores it 
		/// in the local variable list at index 3.
		/// </summary>
		/// <seealso cref="OpCodes.Stloc_3">OpCodes.Stloc_3</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper stloc_3
		{
			get { _ilGenerator.Emit(OpCodes.Stloc_3); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stloc_S"/>, LocalBuilder) that
		/// pops the current value from the top of the evaluation stack and stores it 
		/// in the local variable list at index (short form).
		/// </summary>
		/// <param name="local">A local variable.</param>
		/// <seealso cref="OpCodes.Stloc_S">OpCodes.Stloc_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,LocalBuilder)">ILGenerator.Emit</seealso>
		public EmitHelper stloc_s(LocalBuilder local)
		{
			_ilGenerator.Emit(OpCodes.Stloc_S, local); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stloc_S"/>, byte) that
		/// pops the current value from the top of the evaluation stack and stores it 
		/// in the local variable list at index (short form).
		/// </summary>
		/// <param name="index">A local variable index.</param>
		/// <seealso cref="OpCodes.Stloc_S">OpCodes.Stloc_S</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,short)">ILGenerator.Emit</seealso>
		public EmitHelper stloc_s(byte index)
		{
			_ilGenerator.Emit(OpCodes.Stloc_S, index); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stobj"/>, type) that
		/// copies a value of a specified type from the evaluation stack into a supplied memory address.
		/// </summary>
		/// <param name="type">A Type</param>
		/// <seealso cref="OpCodes.Stobj">OpCodes.Stobj</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Type)">ILGenerator.Emit</seealso>
		public EmitHelper stobj(Type type)
		{
			_ilGenerator.Emit(OpCodes.Stobj, type); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Stsfld"/>, fieldInfo) that
		/// replaces the value of a static field with a value from the evaluation stack.
		/// </summary>
		/// <param name="fieldInfo">A FieldInfo representing a field.</param>
		/// <seealso cref="OpCodes.Stsfld">OpCodes.Stsfld</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,FieldInfo)">ILGenerator.Emit</seealso>
		public EmitHelper stsfld(FieldInfo fieldInfo)
		{
			_ilGenerator.Emit(OpCodes.Stsfld, fieldInfo); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Sub"/>) that
		/// subtracts one value from another and pushes the result onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Sub">OpCodes.Sub</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper sub
		{
			get { _ilGenerator.Emit(OpCodes.Sub); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Sub_Ovf"/>) that
		/// subtracts one integer value from another, performs an overflow check,
		/// and pushes the result onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Sub_Ovf">OpCodes.Sub_Ovf</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper sub_ovf
		{
			get { _ilGenerator.Emit(OpCodes.Sub_Ovf); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Sub_Ovf_Un"/>) that
		/// subtracts one unsigned integer value from another, performs an overflow check,
		/// and pushes the result onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Sub_Ovf_Un">OpCodes.Sub_Ovf_Un</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper sub_ovf_un
		{
			get { _ilGenerator.Emit(OpCodes.Sub_Ovf_Un); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Switch"/>, label[]) that
		/// implements a jump table.
		/// </summary>
		/// <param name="labels">The array of label objects to which to branch from this location.</param>
		/// <seealso cref="OpCodes.Switch">OpCodes.Switch</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label[])">ILGenerator.Emit</seealso>
		public EmitHelper @switch(Label[] labels)
		{
			_ilGenerator.Emit(OpCodes.Switch, labels); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Tailcall"/>) that
		/// performs a postfixed method call instruction such that the current method's stack frame 
		/// is removed before the actual call instruction is executed.
		/// </summary>
		/// <seealso cref="OpCodes.Tailcall">OpCodes.Tailcall</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper tailcall
		{
			get { _ilGenerator.Emit(OpCodes.Tailcall); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Throw"/>) that
		/// throws the exception object currently on the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Throw">OpCodes.Throw</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper @throw
		{
			get { _ilGenerator.Emit(OpCodes.Throw); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Unaligned"/>, label) that
		/// indicates that an address currently atop the evaluation stack might not be aligned 
		/// to the natural size of the immediately following ldind, stind, ldfld, stfld, ldobj, stobj, 
		/// initblk, or cpblk instruction.
		/// </summary>
		/// <param name="label">The label to branch from this location.</param>
		/// <seealso cref="OpCodes.Unaligned">OpCodes.Unaligned</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Label)">ILGenerator.Emit</seealso>
		public EmitHelper unaligned(Label label)
		{
			_ilGenerator.Emit(OpCodes.Unaligned, label); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Unaligned"/>, long) that
		/// indicates that an address currently atop the evaluation stack might not be aligned 
		/// to the natural size of the immediately following ldind, stind, ldfld, stfld, ldobj, stobj, 
		/// initblk, or cpblk instruction.
		/// </summary>
		/// <param name="addr">An address is pushed onto the stack.</param>
		/// <seealso cref="OpCodes.Unaligned">OpCodes.Unaligned</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,long)">ILGenerator.Emit</seealso>
		public EmitHelper unaligned(long addr)
		{
			_ilGenerator.Emit(OpCodes.Unaligned, addr); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Unbox"/>, type) that
		/// converts the boxed representation of a value type to its unboxed form.
		/// </summary>
		/// <param name="type">A Type</param>
		/// <seealso cref="OpCodes.Unbox">OpCodes.Unbox</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Type)">ILGenerator.Emit</seealso>
		public EmitHelper unbox(Type type)
		{
			_ilGenerator.Emit(OpCodes.Unbox, type); return this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Unbox_Any"/>, type) that
		/// converts the boxed representation of a value type to its unboxed form.
		/// </summary>
		/// <param name="type">A Type</param>
		/// <seealso cref="OpCodes.Unbox_Any">OpCodes.Unbox_Any</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode,Type)">ILGenerator.Emit</seealso>
		public EmitHelper unbox_any(Type type)
		{
			_ilGenerator.Emit(OpCodes.Unbox_Any, type);
			return this;
		}

		public EmitHelper unboxIfValueType(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			return type.IsValueType? unbox(type): this;
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Volatile"/>) that
		/// specifies that an address currently atop the evaluation stack might be volatile, 
		/// and the results of reading that location cannot be cached or that multiple stores 
		/// to that location cannot be suppressed.
		/// </summary>
		/// <seealso cref="OpCodes.Volatile">OpCodes.Volatile</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper @volatile
		{
			get { _ilGenerator.Emit(OpCodes.Volatile); return this; }
		}

		/// <summary>
		/// Calls ILGenerator.Emit(<see cref="OpCodes.Xor"/>) that
		/// computes the bitwise XOR of the top two values on the evaluation stack, 
		/// pushing the result onto the evaluation stack.
		/// </summary>
		/// <seealso cref="OpCodes.Xor">OpCodes.Xor</seealso>
		/// <seealso cref="System.Reflection.Emit.ILGenerator.Emit(OpCode)">ILGenerator.Emit</seealso>
		public EmitHelper xor
		{
			get { _ilGenerator.Emit(OpCodes.Xor); return this; }
		}

		/// <summary>
		/// Ends sequence of property calls.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		public void end()
		{
		}

		#endregion

		public EmitHelper LoadInitValue(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			if      (type == typeof(string)) ldsfld(typeof(string).GetField("Empty"));
			else if (type.IsClass ||
			         type.IsInterface)       ldnull.end();
			else if (type == typeof(bool)   ||
			         type == typeof(byte)   ||
			         type == typeof(int)    ||
			         type == typeof(uint)   ||
			         type == typeof(short)  ||
			         type == typeof(ushort) ||
			         type == typeof(sbyte)  ||
			         type == typeof(char))   ldc_i4_0. end();
			else if (type == typeof(double) ||
			         type == typeof(float))  ldc_r4(0).end();
			else if (type == typeof(long)   ||
			         type == typeof(ulong))  ldc_i4_0. conv_i8.end();
			else
				throw new InvalidOperationException();

			return this;
		}

		public bool LoadWellKnownValue(object o)
		{
			if      (o == null)   ldnull.end();
			else if (o is string) ldstr ((string)o);
			else if (o is int)    ldc_i4((int)o);
			else if (o is byte)   ldc_i4((byte)o);
			else if (o is sbyte)  ldc_i4((sbyte)o);
			else if (o is char)   ldc_i4((char)o);
			else if (o is ushort) ldc_i4((ushort)o);
			else if (o is uint)   ldc_i4(unchecked ((int)(uint)o));
			else if (o is ulong)  ldc_i8(unchecked ((long)(ulong)o));
			else if (o is bool)   ldc_i4((bool)o? 1: 0);
			else if (o is short)  ldc_i4((short)o);
			else if (o is long)   ldc_i8((long)o);
			else if (o is float)  ldc_r4((float)o);
			else if (o is double) ldc_r8((double)o);
			else
				return false;

			return true;
		}

		public EmitHelper Init(ParameterInfo parameterInfo, int index)
		{
			if (parameterInfo == null) throw new ArgumentNullException("parameterInfo");

			Type type = TypeHelper.GetUnderlyingType(parameterInfo.ParameterType);

			if (parameterInfo.ParameterType.IsByRef)
			{
				type = type.GetElementType();

				return type.IsValueType && type.IsPrimitive == false?
					ldarg(index).initobj(type):
					ldarg(index).LoadInitValue(type).stind(type);
			}
			else
			{
				return type.IsValueType && type.IsPrimitive == false?
					ldarga(index).initobj(type):
					LoadInitValue(type).starg(index);
			}
		}

		public EmitHelper InitOutParameters(ParameterInfo[] parameters)
		{
			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo pi = parameters[i];

				if (pi.IsOut)
					Init(pi, i + 1);
			}

			return this;
		}

		public EmitHelper Init(LocalBuilder localBuilder)
		{
			if (localBuilder == null) throw new ArgumentNullException("localBuilder");

			Type type = localBuilder.LocalType;

			if (type.IsEnum)
				type = Enum.GetUnderlyingType(type);

			return type.IsValueType && type.IsPrimitive == false?
				ldloca(localBuilder).initobj(type):
				LoadInitValue(type).stloc(localBuilder);
		}

		public EmitHelper LoadType(Type type)
		{
			return ldtoken(type).call(typeof(Type), "GetTypeFromHandle", typeof(RuntimeTypeHandle));
		}

		public EmitHelper CastFromObject(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			if (type == typeof(object)) return this;

			return type.IsValueType? unbox_any(type): castclass(type);
		}

		public EmitHelper conv(Type type)
		{
			if      (type == typeof(byte))   conv_u1.end();
			else if (type == typeof(char))   conv_u2.end();
			else if (type == typeof(ushort)) conv_u2.end();
			else if (type == typeof(uint))   conv_u4.end();
			else if (type == typeof(ulong))  conv_i8.end();
			else if (type == typeof(bool))   conv_i1.end();
			else if (type == typeof(sbyte))  conv_i1.end();
			else if (type == typeof(short))  conv_i2.end();
			else if (type == typeof(int))    conv_i4.end();
			else if (type == typeof(long))   conv_i8.end();
			else if (type == typeof(float))  conv_r4.end();
			else if (type == typeof(double)) conv_r8.end();

			return this;
		}

		public EmitHelper unboxOrCast(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			return type.IsValueType? unbox(type): castclass(type);
		}

		public void AddMaxStackSize(int size)
		{
			FieldInfo fi = _ilGenerator.GetType().GetField(
				"m_maxStackSize", BindingFlags.Instance | BindingFlags.NonPublic);

			if (fi != null)
			{
				size += (int)fi.GetValue(_ilGenerator);
				fi.SetValue(_ilGenerator, size);
			}
		}

        public static BindingFlags AnyVisibilityInstance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public EmitHelper GenerateSetMember(MemberInfo memberInfo)
        {
            if (memberInfo.MemberType == MemberTypes.Field)
            {
                FieldInfo fieldInfo = memberInfo.DeclaringType.GetField(memberInfo.Name);
                return stfld(fieldInfo);
            }
            if (memberInfo.MemberType == MemberTypes.Property)
            {
                PropertyInfo propertyInfo = memberInfo.DeclaringType.GetProperty(memberInfo.Name);
                MethodInfo miPropertySet = propertyInfo.GetSetMethod();
                return callvirt(miPropertySet);
            }
            return this;
        }

        public EmitHelper GeneratePrimitiveCast(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.SByte:
                    conv_i1.end();
                    break;
                case TypeCode.Byte:
                    conv_u1.end();
                    break;
                case TypeCode.Int16:
                    conv_i2.end();
                    break;
                case TypeCode.UInt16:
                    conv_u2.end();
                    break;
                case TypeCode.Int32:
                    conv_i4.end();
                    break;
                case TypeCode.UInt32:
                    conv_u4.end();
                    break;
                case TypeCode.Int64:
                    conv_i8.end();
                    break;
                case TypeCode.UInt64:
                    conv_u8.end();
                    break;
                case TypeCode.Single:
                    conv_r4.end();
                    break;
                case TypeCode.Double:
                    conv_r8.end();
                    break;
                case TypeCode.Decimal:
                    {
                        MethodInfo miDecimalOpExplicit = typeof(Decimal).GetMethod("op_Explicit", new Type[] { typeof(double) });
                        conv_r8.call(miDecimalOpExplicit).end();
                    }
                    break;
                case TypeCode.Boolean:
                    break;
            }
            return this;
        }

        public EmitHelper GenerateThrowUnexpectedAMFException(MemberInfo memberInfo)
        {
            ConstructorInfo exceptionConstructor = typeof(FluorineFx.Exceptions.UnexpectedAMF).GetConstructor(AnyVisibilityInstance, null, CallingConventions.HasThis, new Type[] { typeof(string) }, null);
            ldstr(string.Format("Unexpected data for member {0}", memberInfo.Name))
            .newobj(exceptionConstructor)
            .@throw
            .end();
            return this;
        }

        public EmitHelper GenerateStoreElementFromObject(Type memberType)
        {
            switch (Type.GetTypeCode(memberType))
            {
                case TypeCode.SByte:
                    unbox_any(typeof(double)).conv_i1.stelem_i1.end();
                    break;
                case TypeCode.Byte:
                    unbox_any(typeof(double)).conv_u1.stelem_i1.end();
                    break;
                case TypeCode.Int16:
                    unbox_any(typeof(double)).conv_i2.stelem_i2.end();
                    break;
                case TypeCode.UInt16:
                    unbox_any(typeof(double)).conv_u2.stelem_i2.end();
                    break;
                case TypeCode.Int32:
                    unbox_any(typeof(double)).conv_i4.stelem_i4.end();
                    break;
                case TypeCode.UInt32:
                    unbox_any(typeof(double)).conv_u4.stelem_i4.end();
                    break;
                case TypeCode.Int64:
                    unbox_any(typeof(double)).conv_i8.stelem_i8.end();
                    break;
                case TypeCode.UInt64:
                    unbox_any(typeof(double)).conv_u8.stelem_i8.end();
                    break;
                case TypeCode.Single:
                    unbox_any(typeof(double)).conv_r4.stelem_r4.end();
                    break;
                case TypeCode.Double:
                    unbox_any(typeof(double)).conv_r8.stelem_r8.end();
                    break;
                case TypeCode.Decimal:
                    {
                        MethodInfo miDecimalOpExplicit = typeof(Decimal).GetMethod("op_Explicit", new Type[] { typeof(double) });
                        unbox_any(typeof(double)).call(miDecimalOpExplicit).stobj(typeof(decimal)).end();
                    }
                    break;
                case TypeCode.Boolean:
                    unbox_any(typeof(double)).conv_i4.stelem_i4.end()
                    ;
                    break;
                case TypeCode.Char:
                    throw new NotSupportedException();
                default:
                    stobj(memberType).end();
                    break;
            }
            return this;
        }

        public EmitHelper GenerateNullValueAccess(MemberInfo memberInfo, Type memberType, Label labelStart)
        {
            MethodInfo miFluorineConfigurationGetInstance = typeof(FluorineFx.Configuration.FluorineConfiguration).GetMethod("get_Instance");
            MethodInfo miFluorineConfigurationGetNullableValues = typeof(FluorineFx.Configuration.FluorineConfiguration).GetMethod("get_NullableValues");
            MethodInfo miTypeGetTypeFromHandle = typeof(Type).GetMethod("GetTypeFromHandle");
            MethodInfo miNullableTypeCollectionGetItem = typeof(FluorineFx.Configuration.NullableTypeCollection).GetMethod("get_Item", new Type[] { typeof(Type) });
            
            MarkLabel(labelStart)
            //instance.member = (type)FluorineConfiguration.Instance.NullableValues[typeof(member)];
            .ldloc_0 //Push 'instance'
            .call(miFluorineConfigurationGetInstance)
            .callvirt(miFluorineConfigurationGetNullableValues)
            .ldtoken(memberType)
            .call(miTypeGetTypeFromHandle)
            .callvirt(miNullableTypeCollectionGetItem)
            .unbox_any(memberType)
            .end();
            return this;
        }

	}
}
