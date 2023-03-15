namespace AutoTrust;
using System.Xml;
using System.Xml.Schema;

#pragma warning disable

// http://www.tkachenko.com/blog/archives/000585.html

public class XmlWrappingReader : XmlReader, IXmlLineInfo {
  protected XmlReader reader;
  protected IXmlLineInfo readerAsIXmlLineInfo;

  public XmlWrappingReader(XmlReader baseReader) => this.Reader = baseReader;

  public override void Close() => this.reader?.Close();

  protected override void Dispose(bool disposing) => ((IDisposable)this.reader).Dispose();

  public override string GetAttribute(int i) => this.reader.GetAttribute(i);

  public override string GetAttribute(string name) => this.reader.GetAttribute(name);

  public override string GetAttribute(string name, string namespaceURI) => this.reader.GetAttribute(name, namespaceURI);

  public virtual bool HasLineInfo() {
    if (this.readerAsIXmlLineInfo != null) {
      return this.readerAsIXmlLineInfo.HasLineInfo();
    }
    return false;
  }

  public override string LookupNamespace(string prefix) => this.reader.LookupNamespace(prefix);

  public override void MoveToAttribute(int i) => this.reader.MoveToAttribute(i);

  public override bool MoveToAttribute(string name) => this.reader.MoveToAttribute(name);

  public override bool MoveToAttribute(string name, string ns) => this.reader.MoveToAttribute(name, ns);

  public override bool MoveToElement() => this.reader.MoveToElement();

  public override bool MoveToFirstAttribute() => this.reader.MoveToFirstAttribute();

  public override bool MoveToNextAttribute() => this.reader.MoveToNextAttribute();

  public override bool Read() => this.reader.Read();

  public override bool ReadAttributeValue() => this.reader.ReadAttributeValue();

  public override void ResolveEntity() => this.reader.ResolveEntity();

  public override void Skip() => this.reader.Skip();


  public override int AttributeCount => this.reader.AttributeCount;

  public override string BaseURI => this.reader.BaseURI;

  public override bool CanResolveEntity => this.reader.CanResolveEntity;

  public override int Depth => this.reader.Depth;

  public override bool EOF => this.reader.EOF;

  public override bool HasAttributes => this.reader.HasAttributes;

  public override bool HasValue => this.reader.HasValue;

  public override bool IsDefault => this.reader.IsDefault;

  public override bool IsEmptyElement => this.reader.IsEmptyElement;

  public virtual int LineNumber {
    get {
      if (this.readerAsIXmlLineInfo != null) {
        return this.readerAsIXmlLineInfo.LineNumber;
      }
      return 0;
    }
  }

  public virtual int LinePosition {
    get {
      if (this.readerAsIXmlLineInfo != null) {
        return this.readerAsIXmlLineInfo.LinePosition;
      }
      return 0;
    }
  }

  public override string LocalName => this.reader.LocalName;

  public override string Name => this.reader.Name;

  public override string NamespaceURI => this.reader.NamespaceURI;

  public override XmlNameTable NameTable => this.reader.NameTable;

  public override XmlNodeType NodeType => this.reader.NodeType;

  public override string Prefix => this.reader.Prefix;

  public override char QuoteChar => this.reader.QuoteChar;

  protected XmlReader Reader {
    get => this.reader;
    set {
      this.reader = value;
      this.readerAsIXmlLineInfo = value as IXmlLineInfo;
    }
  }

  public override ReadState ReadState => this.reader.ReadState;

  public override IXmlSchemaInfo SchemaInfo => this.reader.SchemaInfo;

  public override XmlReaderSettings Settings => this.reader.Settings;

  public override string Value => this.reader.Value;

  public override Type ValueType => this.reader.ValueType;

  public override string XmlLang => this.reader.XmlLang;

  public override XmlSpace XmlSpace => this.reader.XmlSpace;
}
