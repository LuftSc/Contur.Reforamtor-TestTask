<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:xs="http://www.w3.org/2001/XMLSchema"
                xmlns:array="http://www.w3.org/2005/xpath-functions/array"
                xmlns:map="http://www.w3.org/2005/xpath-functions/map"
                xmlns:math="http://www.w3.org/2005/xpath-functions/math"
                xmlns:employee="http://employees.com/transform-to-employee"
                exclude-result-prefixes="#all"
                expand-text="yes"
                version="3.0">

  <xsl:output method="xml" indent="yes" encoding="UTF-8"/>
  
  <xsl:function name="employee:normalize-amount" as="xs:double">
    <xsl:param name="amount-string" as="xs:string"/>
    <xsl:sequence select="number(
        translate($amount-string, ',', '.')
    )"/>
  </xsl:function>
  
  <xsl:template match="/Pay">
    <Employees>
      <xsl:for-each-group select="//item" group-by="concat(@name, '|', @surname)">
        <Employee name="{current-group()[1]/@name}" surname="{current-group()[1]/@surname}">
          <xsl:for-each select="current-group()">
            <xsl:sort select="employee:normalize-amount(@amount)" order="ascending" />
            <salary amount="{@amount}"> 
              <xsl:attribute name="mount">
                <xsl:choose>
                  <xsl:when test="parent::*[name() = ('january','february','march','april','may','june','july','august','september','october','november','december')]">
                   <xsl:value-of select="local-name(..)"/>
                  </xsl:when>
                  <xsl:otherwise>
                   <xsl:value-of select="@mount"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:attribute> 
            </salary>
          </xsl:for-each>
        </Employee>
      </xsl:for-each-group>
    </Employees>
  </xsl:template>
</xsl:stylesheet>