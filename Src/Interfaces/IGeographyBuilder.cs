using System.Collections.Generic;

public interface IGeographyBuilder
{
  IGeographyBuilder HavingState (IEnumerable<string> states);
  IGeographyBuilder HavingZipCode (string zipCode);
  IGeographyBuilder WithinZipRadius(int zipRadius);
  IGeographyBuilder HavingMSA(IEnumerable<string> msa);
  IGeographyBuilder HavingCity(IEnumerable<string> cities);
  IGeographyBuilder HavingCounty(IEnumerable<string> counties);
}